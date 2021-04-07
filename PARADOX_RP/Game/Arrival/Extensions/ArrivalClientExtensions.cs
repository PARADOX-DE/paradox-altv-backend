using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Arrival.Extensions
{
    public static class ArrivalClientExtensions
    {
        public static bool IsPlayerArrived(this PXPlayer player)
        {
            return false;
        }

        public static void PlayArrivalCutscene(this PXPlayer player)
        {
            player.Emit("Arrival::PlayCutscene");
        }
    }
}
