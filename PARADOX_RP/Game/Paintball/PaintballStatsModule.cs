using AltV.Net.Async;
using AltV.Net.Async.Elements.Refs;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Events;
using PARADOX_RP.Core.Events.Intervals;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Lobby;
using PARADOX_RP.Game.MiniGames.Models;
using PARADOX_RP.Game.Misc.Position;
using PARADOX_RP.Game.Paintball.Models;
using PARADOX_RP.Utils;
using PARADOX_RP.Utils.Callbacks;
using PARADOX_RP.Utils.Enums;
using PARADOX_RP.Utils.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Paintball
{
    public enum PaintballRankTypes
    {
        BRONZE = 1,
        SILVER,
        GOLD,
        PLATIN,
        DIAMOND,
        ELITE
    }

    public sealed class PaintballStatsModule : Module<PaintballStatsModule>, IEventModuleLoad
    {
        private readonly PXContext _pxContext;
        private readonly ILogger _logger;

        public Dictionary<int, PaintballStats> _stats = new Dictionary<int, PaintballStats>();

        public PaintballStatsModule(PXContext pxContext, ILogger logger) : base("PaintballStats")
        {
            _pxContext = pxContext;
            _logger = logger;

            LoadDatabaseTable<PaintballStats>(_pxContext.PaintballStats, (stat) => _stats.Add(stat.PlayerId, stat));
        }

        public void OnModuleLoad()
        {
            var Settings = _pxContext.PaintballSettings.FirstOrDefault();
            if (Settings.LastPointDrop.AddHours(24) > DateTime.Now) return;

            Settings.LastPointDrop = DateTime.Now;

            _pxContext.PaintballStats.ForEach((stats) => stats.LeaguePoints -= Settings.DAILY_LOST_LEAGUE_POINTS);
            _pxContext.SaveChanges();

            _logger.Console(ConsoleLogType.SUCCESS, "Paintball Stats", $"Alle Stats wurden um {Settings.DAILY_LOST_LEAGUE_POINTS} verringert.");
        }

        public bool StatsExists(int playerId) => _stats.ContainsKey(playerId);

        public PaintballStats GetStatsByPlayerId(int playerId)
        {
            if (_stats.TryGetValue(playerId, out PaintballStats stats))
                return stats;

            return null;
        }

        public async void CreateUserStats(PXPlayer player)
        {
            if (_stats.ContainsKey(player.SqlId)) return;

            await using var pxContext = new PXContext();

            var toInsert = new PaintballStats(player.SqlId);
            await pxContext.PaintballStats.AddAsync(toInsert);

            await pxContext.SaveChangesAsync();

            _stats.Add(player.SqlId, toInsert);
        }
    }
}
