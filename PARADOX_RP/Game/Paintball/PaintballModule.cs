using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Async.Elements.Refs;
using AltV.Net.Elements.Entities;
using EntityStreamer;
using Microsoft.EntityFrameworkCore;
using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Events.Intervals;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Lobby;
using PARADOX_RP.Game.Paintball.Models;
using PARADOX_RP.Utils.Callbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Paintball
{
    public sealed class PaintballModule : Module<PaintballModule>, IEventIntervalMinute
    {
        private readonly PXContext _pxContext;
        private readonly IEventController _eventController;

        public Dictionary<int, PaintballStats> _stats = new Dictionary<int, PaintballStats>();
        public Dictionary<int, PaintballRanks> _ranks = new Dictionary<int, PaintballRanks>();
        public Dictionary<int, PaintballGameMap> _maps = new Dictionary<int, PaintballGameMap>();

        // Settings
        private const int MAX_ROUND_PLAYERS = 8;

        private readonly List<PXPlayer> _inQueuePlayers = new List<PXPlayer>();

        public PaintballModule(PXContext pxContext, IEventController eventController) : base("Paintball")
        {
            _pxContext = pxContext;
            _eventController = eventController;

            LoadDatabaseTable<PaintballRanks>(_pxContext.PaintballRanks, (rank) => _ranks.Add(rank.Id, rank));
            LoadDatabaseTable<PaintballStats>(_pxContext.PaintballStats, (stat) => _stats.Add(stat.Id, stat));
            LoadDatabaseTable<PaintballMaps>(_pxContext.PaintballMaps.Include(f => f.Flags)
                                                                    .Include(s => s.Spawns), OnLoadMap);

            _eventController.OnClient<PXPlayer>("SearchPaintballLobby", SearchLobby);
        }

        private void OnLoadMap(PaintballMaps dbPainballMap)
        {
            var paintballGameMap = new PaintballGameMap(dbPainballMap);

            foreach (var paintballFlag in dbPainballMap.Flags)
            {
                IColShape flagColShape = Alt.CreateColShapeCylinder(paintballFlag.Position, 3.5f, 3.5f);
                Marker flagMarker = MarkerStreamer.Create(MarkerTypes.MarkerTypeCheckeredFlagRect, paintballFlag.Position, new Vector3(0, 0, 0));

                paintballGameMap.ColShapes.Add(flagColShape);
                paintballGameMap.Markers.Add(flagMarker);
            }

            _maps.Add(dbPainballMap.Id, paintballGameMap);
        }

        public void SearchLobby(PXPlayer player)
        {
            var foundLobby = _maps.Values.FirstOrDefault(i => i.LobbyStatus == LobbyStatus.WAITING && i.Players.Count < MAX_ROUND_PLAYERS);
            if (foundLobby == null)
            {
                // queue player
                _inQueuePlayers.Add(player);
                return;
            }

            EnterLobby(player, foundLobby);
        }

        public async void EnterLobby(PXPlayer player, PaintballGameMap paintballMap)
        {
            paintballMap.Players.Add(player);
            player.SendNotification(ModuleName, $"Lobby gefunden! Map: {paintballMap.Data.Name}", NotificationTypes.SUCCESS);

            await player.SetPositionAsync(paintballMap.Data.QueuePosition);
        }

        // Interval ist dazu da, die Runden aufzufüllen die noch gequeued werden.
        public async Task OnEveryMinute()
        {
            var asyncCallback = new AsyncFunctionCallback<PXPlayer>(async (player) =>
            {
                player.SendNotification("Paintball", "Lobby wird gesucht...", NotificationTypes.ERROR);
                var foundLobby = _maps.Values.FirstOrDefault(i => i.LobbyStatus == LobbyStatus.WAITING && i.Players.Count < MAX_ROUND_PLAYERS);
                if (foundLobby == null) return;

                _inQueuePlayers.Remove(player);
                EnterLobby(player, foundLobby);

                await Task.CompletedTask;
            });

            foreach (var entity in _inQueuePlayers)
            {
                using var entityRef = new AsyncPlayerRef(entity);
                if (!entityRef.Exists) continue;

                entityRef.DebugCountUp();
                await asyncCallback.OnBaseObject(entity);
                entityRef.DebugCountDown();
            }
        }
    }
}
