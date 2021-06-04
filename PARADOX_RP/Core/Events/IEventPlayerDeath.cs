using PARADOX_RP.Core.Factories;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Events
{
    interface IEventPlayerDeath
    {
        bool Enabled { get; }
        void OnPlayerDeath(PXPlayer player, PXPlayer killer, DeathReasons deathReason, uint weapon);
    }
}
