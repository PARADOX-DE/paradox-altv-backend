using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Enums;
using PARADOX_RP.Controllers.Weapon.Interface;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Controllers.Weapon
{
    class WeaponController : IWeaponController
    {
        public async Task LoadWeapons(PXPlayer player, IEnumerable<PlayerWeapons> weapons = null)
        {
            IEnumerable<PlayerWeapons> _weapons = weapons;
            if (_weapons == null) _weapons = player.PlayerWeapons;

            foreach (var weapon in weapons)
            {
                await player.RemoveAllWeaponsAsync();
                await player.GiveWeaponAsync((uint)weapon.WeaponHash, weapon.Ammo, false);
            }
        }
        public async Task AddWeapon(PXPlayer player, WeaponModel weapon)
        {
            await player.GiveWeaponAsync((uint)weapon, 0, true);
        }
    }
}
