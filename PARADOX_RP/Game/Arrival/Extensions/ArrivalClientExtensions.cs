using AltV.Net.Async;
using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Arrival.Extensions
{
    public static class ArrivalClientExtensions
    {
        public static bool IsPlayerArrived(this PXPlayer player)
        {
            return false;
        }

        public static Task PlayArrivalCutscene(this PXPlayer player)
        {
            return player.EmitAsync("StartArrivalCutscene");
        }
    }
}
