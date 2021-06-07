using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Core.Events
{
    interface IEventPlayerWeaponChange
    {
        bool Enabled { get; }
        Task OnPlayerWeaponChange(PXPlayer player, uint oldWeapon, uint newWeapon);
    }
}
