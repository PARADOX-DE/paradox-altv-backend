using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Async.Elements.Refs;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using EntityStreamer;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Controllers.Weapon.Interface;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Events;
using PARADOX_RP.Core.Events.Intervals;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Lobby;
using PARADOX_RP.Game.Paintball.Models;
using PARADOX_RP.Utils.Callbacks;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Paintball
{
    public sealed class PaintballModule : Module<PaintballModule>, IEventIntervalMinute, IEventModuleLoad,
                                                                     IEventKeyPressed
    {
        private readonly PXContext _pxContext;
        private readonly IEventController _eventController;
        private readonly IWeaponController _weaponController;

        public Dictionary<int, PaintballRanks> _ranks = new Dictionary<int, PaintballRanks>();
        public Dictionary<int, PaintballGameMap> _maps = new Dictionary<int, PaintballGameMap>();

        public PaintballSettings Settings;

        private readonly List<PXPlayer> _inQueuePlayers = new List<PXPlayer>();

        private readonly List<Position> _stationPoints = new List<Position>()
        {
            new Position(-245.3011f, -2003.011f, 30.13916f)
        };

        public PaintballModule(PXContext pxContext, IEventController eventController, IWeaponController weaponController) : base("Paintball")
        {
            _pxContext = pxContext;
            _eventController = eventController;
            _weaponController = weaponController;

            _eventController.OnClient<PXPlayer>("SearchPaintballLobby", SearchLobby);
        }

        public void OnModuleLoad()
        {
            Settings = _pxContext.PaintballSettings.FirstOrDefault();
            _stationPoints.ForEach((p) => MarkerStreamer.Create(MarkerTypes.MarkerTypeDebugSphere, p, new Vector3(1, 1, 1)));

            LoadDatabaseTable<PaintballRanks>(_pxContext.PaintballRanks, (rank) => _ranks.Add(rank.Id, rank));
            LoadDatabaseTable<PaintballMaps>(_pxContext.PaintballMaps.Include(f => f.Flags)
                                                                    .Include(s => s.Spawns), OnLoadMap);
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

            foreach (var paintballSpawn in dbPainballMap.Spawns)
            {
                Marker spawnMarker = MarkerStreamer.Create(MarkerTypes.MarkerTypeCheckeredFlagRect, paintballSpawn.Position, new Vector3(0, 0, 0));

                paintballGameMap.Markers.Add(spawnMarker);
            }

            _maps.Add(dbPainballMap.Id, paintballGameMap);
        }

        public async Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key)
        {
            if (key != KeyEnumeration.E) return await Task.FromResult(false);
            var playerPos = Position.Zero; player.GetPositionLocked(ref playerPos);

            var stationPoint = _stationPoints.FirstOrDefault(s => s.Distance(playerPos) <= 4);
            if (stationPoint == default) return await Task.FromResult(false);

            // Create Stats 
            if (!PaintballStatsModule.Instance.StatsExists(player.SqlId))
            {
                await _weaponController.AddWeapon(player, WeaponModel.AdvancedRifle);
                await  _weaponController.AddWeapon(player, WeaponModel.Pistol);

                PaintballStatsModule.Instance.CreateUserStats(player);
            }

            // WindowController => Warzone lobby?
            SearchLobby(player);

            return await Task.FromResult(true);
        }

        public void SearchLobby(PXPlayer player)
        {
            player.SendNotification("Paintball", "Lobby wird gesucht...", NotificationTypes.ERROR);

            var foundLobby = _maps.Values.FirstOrDefault(i => i.LobbyStatus == LobbyStatus.WAITING && i.Players.Count < Settings.MAX_ROUND_PLAYERS);
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
            paintballMap.Players.Add(player.SqlId, new PaintballGamePlayer(player));
            foreach (var paintballDebug in paintballMap.Players)
                AltAsync.Log($"{paintballDebug.Key} - {paintballDebug.Value.Target.Username}");

            player.SendNotification(ModuleName, $"Lobby gefunden! Map: {paintballMap.Data.Name}", NotificationTypes.SUCCESS);

            player.DimensionType = DimensionTypes.PAINTBALL;

            await player.SetDimensionAsync(paintballMap.Data.Id);
            await player.SetPositionAsync(paintballMap.Data.QueuePosition);
        }

        // Interval ist dazu da, die Runden aufzufüllen die noch gequeued werden.
        public async Task OnEveryMinute()
        {
            var asyncCallback = new AsyncFunctionCallback<PXPlayer>(async (player) =>
            {
                player.SendNotification("Paintball", "Lobby wird gesucht...", NotificationTypes.ERROR);
                var foundLobby = _maps.Values.FirstOrDefault(i => i.LobbyStatus == LobbyStatus.WAITING && i.Players.Count < Settings.MAX_ROUND_PLAYERS);
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

        public PaintballGameMap GetPaintballGameMapByPlayer(PXPlayer player) => _maps.Values.FirstOrDefault(m => m.GetPlayerById(player.SqlId) != null);

    }
}
