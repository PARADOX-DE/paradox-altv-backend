using AltV.Net.Async;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Paintball.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Paintball
{
    
    public sealed class PaintballGameModule : Module<PaintballGameModule>
    {
        public PaintballGameModule() : base("PaintballGame") { }


        public void StartupGame(PaintballGameMap paintballMap)
        {
            // First clean up old lobby!
            foreach(var player in paintballMap.Players)
            {
                paintballMap.Players.Remove(player);
                player.set
            }
        }

        public void FinishGame(PaintballMaps paintballMap)
        {

        }
    }
}
