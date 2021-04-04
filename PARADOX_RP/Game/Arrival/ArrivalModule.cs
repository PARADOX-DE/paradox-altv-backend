using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Arrival.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Arrival
{
    class Arrival : ModuleBase<Arrival>
    {

        public Arrival() : base("Arrival")
        {

        }

        public void NewPlayerArrival(PXPlayer player)
        {
            if (player.IsPlayerArrived()) return;
        }
    }
}
