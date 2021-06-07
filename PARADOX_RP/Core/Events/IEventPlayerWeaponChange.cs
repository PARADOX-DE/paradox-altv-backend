using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Events
{
    interface IEventPlayerWeaponChange
    {
        bool Enabled { get; }
        void OnPlayerWeaponChange(PXPlayer player, uint oldWeapon, uint newWeapon);
    }
}
