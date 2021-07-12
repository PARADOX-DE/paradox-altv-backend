using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Paintball.Extensions
{
    public static class PaintballStatsExtensions
    {
        public static PaintballStats GetPaintballStats(this PXPlayer player) => PaintballStatsModule.Instance.GetStatsByPlayerId(player.SqlId);
    }
}
