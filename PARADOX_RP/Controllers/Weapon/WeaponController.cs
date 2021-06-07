using AltV.Net.Data;
using AltV.Net.Enums;
using PARADOX_RP.Controllers.Weapon.Interface;
using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Controllers.Weapon
{
    class WeaponController : IWeaponController
    {
        public async Task AddWeapon(PXPlayer player, WeaponModel weapon)
        {
            player.GiveWeapon((uint)weapon, 0, true);
        }
    }
}
