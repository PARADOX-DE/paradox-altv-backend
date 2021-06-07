using AltV.Net.Data;
using AltV.Net.Enums;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Controllers.Weapon.Interface
{
    interface IWeaponController
    {
        Task LoadWeapons(PXPlayer player, IEnumerable<PlayerWeapons> weapons = null);
        Task AddWeapon(PXPlayer player, WeaponModel weapon);

    }
}
