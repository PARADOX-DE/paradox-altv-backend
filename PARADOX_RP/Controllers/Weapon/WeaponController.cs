using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Enums;
using Microsoft.EntityFrameworkCore;
using PARADOX_RP.Controllers.Weapon.Interface;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Controllers.Weapon
{
    class WeaponController : IWeaponController
    {
        public async Task LoadWeapons(PXPlayer player, ICollection<PlayerWeapons> weapons = null)
        {
            ICollection<PlayerWeapons> _weapons = weapons;
            if (_weapons == null) _weapons = player.PlayerWeapons;
            else player.PlayerWeapons = _weapons;

            await player.RemoveAllWeaponsAsync();

            foreach (var weapon in _weapons)
            {
                await player.GiveWeaponAsync((uint)weapon.WeaponHash, weapon.Ammo, false);
            }
        }
        public async Task AddWeapon(PXPlayer player, WeaponModel weapon)
        {
            await using (var px = new PXContext())
            {
                PlayerWeapons dbWeapon = await px.PlayerWeapons.FirstOrDefaultAsync(p => p.PlayerId == player.SqlId && p.WeaponHash == weapon);
                if (dbWeapon != null) return;

                var weaponInsert = new PlayerWeapons()
                {
                    PlayerId = player.SqlId,
                    WeaponHash = weapon,
                    Ammo = 1337
                };

                await px.PlayerWeapons.AddAsync(weaponInsert);
                player.PlayerWeapons.Add(weaponInsert);

                await px.SaveChangesAsync();
            }

            await player.GiveWeaponAsync((uint)weapon, 1337, true);
        }

        public async Task<bool> AddAmmo(PXPlayer player, WeaponModel weapon, int ammo)
        {
            if (player.CurrentWeapon == (uint)weapon) return false;
            

            await using (var px = new PXContext())
            {
                PlayerWeapons dbWeapon = await px.PlayerWeapons.FirstOrDefaultAsync(p => p.PlayerId == player.SqlId && p.WeaponHash == weapon);
                if (dbWeapon == null) return false;

                dbWeapon.Ammo += ammo;
                await player.GiveWeaponAsync((uint)weapon, ammo, false);

                await px.SaveChangesAsync();
            }

            return true;
        }
    }
}
