using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Paintball.Models
{
    public class PaintballGamePlayer
    {
        public PaintballGamePlayer(PXPlayer target)
        {
            Target = target;

            Kills = 0;
            Deaths = 0;
            Killstreak = 0;
        }

        public PXPlayer Target { get; set; }

        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Killstreak { get; set; }
    }
}
