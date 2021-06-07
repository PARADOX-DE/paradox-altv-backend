using AltV.Net.Data;
using AltV.Net.Enums;
using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Controllers.Weapon.Interface
{
    interface IWeaponController
    {
        Task AddWeapon(PXPlayer player, WeaponModel weapon);
    }
}
