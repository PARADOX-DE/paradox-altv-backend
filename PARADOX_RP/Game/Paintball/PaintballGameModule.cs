using AltV.Net.Async;
using AltV.Net.Async.Elements.Refs;
using PARADOX_RP.Controllers.Inventory;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Events;
using PARADOX_RP.Core.Events.Intervals;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Injury.Extensions;
using PARADOX_RP.Game.Injury.Interfaces;
using PARADOX_RP.Game.Inventory;
using PARADOX_RP.Game.Lobby;
using PARADOX_RP.Game.MiniGames.Models;
using PARADOX_RP.Game.Misc.Position;
using PARADOX_RP.Game.Paintball.Extensions;
using PARADOX_RP.Game.Paintball.Models;
using PARADOX_RP.Utils.Callbacks;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Paintball
{

    public sealed class PaintballGameModule : Module<PaintballGameModule>, IEventIntervalMinute, IEventPlayerDeath,
                                                                            ISpecialInjury
    {
        private readonly InventoryModule _inventoryModule;
        private readonly IInventoryController _inventoryController;

        public PaintballGameModule(InventoryModule inventoryModule, IInventoryController inventoryController) : base("PaintballGame")
        {
            _inventoryModule = inventoryModule;
            _inventoryController = inventoryController;
        }

        public async Task OnEveryMinute()
        {
            foreach (PaintballGameMap paintballMap in PaintballModule.Instance._maps.Values)
            {
                // Handle all waiting Lobbys
                if (paintballMap.LobbyStatus == LobbyStatus.WAITING)
                {
                    if (paintballMap.Players.Count >= PaintballModule.Instance.Settings.MIN_PLAYERS_START_ROUND)
                    {
                        await StartupGame(paintballMap);
                        return;
                    }

                    var asyncCallback = new AsyncFunctionCallback<PXPlayer>(async (player) =>
                    {
                        player.SendNotification("Paintball", "Suche nach Spielern...", NotificationTypes.SUCCESS);

                        await Task.CompletedTask;
                    });

                    foreach (var entity in paintballMap.Players.Values)
                    {
                        using var entityRef = new AsyncPlayerRef(entity.Target);
                        if (!entityRef.Exists) continue;

                        entityRef.DebugCountUp();
                        await asyncCallback.OnBaseObject(entity.Target);
                        entityRef.DebugCountDown();
                    }
                }
            }
        }

        public async Task StartupGame(PaintballGameMap paintballMap)
        {
            foreach (KeyValuePair<int, PaintballGamePlayer> keyValuePair in paintballMap.Players)
            {
                PXPlayer player = keyValuePair.Value.Target;
                if (player == null) continue;

                // set to paintball round
                await player.SetPositionAsync(paintballMap.Data.Spawns.PickRandom().Position);
                player.SendNotification("Paintball", "Die Runde hat begonnen!", NotificationTypes.SUCCESS);

                if (!_inventoryModule._items.TryGetValue(2, out Items ItemVest)) return;
                if (!_inventoryModule._items.TryGetValue(4, out Items ItemMedkit)) return;

                await _inventoryController.CreateItem(player.Inventory, 2, 5, "Paintball");
                await _inventoryController.CreateItem(player.Inventory, 4, 10, "Paintball");

                player.Minigame = MinigameTypes.PAINTBALL;

                await player.SetHealthAsync(player.MaxHealth);
                await player.SetArmorAsync(player.MaxArmor);
            }

            paintballMap.LobbyStatus = LobbyStatus.INGAME;

        }

        public void FinishGame(PaintballGameMap paintballMap, PaintballGamePlayer winner)
        {
            IOrderedEnumerable<PaintballGamePlayer> sortedScoreboard = paintballMap.Players.Values.OrderBy(p => p.Kills)
                                                                                         .ThenBy(p => p.Killstreak)
                                                                                         .ThenBy(p => p.Deaths);

            var asyncCallback = new AsyncFunctionCallback<PXPlayer>(async (player) =>
            {
                if (player.SqlId == winner.Target.SqlId)
                    player.SendNotification("Paintball", "Du hast gewonnen!", NotificationTypes.SUCCESS);
                else
                    player.SendNotification("Paintball", $"{winner.Target.Username} hat gewonnen!", NotificationTypes.SUCCESS);

                await player.SetPositionAsync(PositionModule.Instance.Get(Positions.PAINTBALL_ROUND_END));
                await player.SetDimensionAsync(0);
                player.DimensionType = DimensionTypes.WORLD;
                player.Minigame = MinigameTypes.NONE;

                var paintballStats = paintballMap.GetPlayerById(player.SqlId);
                if (paintballStats == null) return;

                AltAsync.Log($"{player.Username} [K: {paintballStats.Kills}] [D: {paintballStats.Deaths}] [S: {paintballStats.Killstreak}]");

                paintballMap.Players.Remove(player.SqlId);
                paintballMap.LobbyStatus = LobbyStatus.WAITING;

                await Task.CompletedTask;
            });

            foreach (var entity in paintballMap.Players.Values)
            {
                using var entityRef = new AsyncPlayerRef(entity.Target);
                if (!entityRef.Exists) continue;

                entityRef.DebugCountUp();
                asyncCallback.OnBaseObject(entity.Target);
                entityRef.DebugCountDown();
            }
        }


        public bool VerifyGameWin(PaintballGamePlayer player)
        {
            bool winResult = false;

            switch (player.Kills)
            {
                case int kills when kills >= 10:
                    winResult = true;
                    player.Target.SendNotification("Paintball", "Du hast gewonnen!", NotificationTypes.SUCCESS);
                    break;

                case 8:
                    player.Target.SendNotification("Paintball", "Du brauchst noch 2 Kills bis zum Sieg!", NotificationTypes.SUCCESS);
                    break;

                case 6:
                    player.Target.SendNotification("Paintball", "Du brauchst noch 4 Kills bis zum Sieg!", NotificationTypes.SUCCESS);
                    break;

                case 4:
                    player.Target.SendNotification("Paintball", "Du brauchst noch 6 Kills bis zum Sieg!", NotificationTypes.SUCCESS);
                    break;

                case 2:
                    player.Target.SendNotification("Paintball", "Du brauchst noch 8 Kills bis zum Sieg!", NotificationTypes.SUCCESS);
                    break;
            }

            return winResult;
        }

        public async void OnPlayerDeath(PXPlayer player, PXPlayer killer, DeathReasons deathReason, uint weapon)
        {
            if (player.Minigame != MinigameTypes.PAINTBALL) return;

            var paintballMap = PaintballModule.Instance.GetPaintballGameMapByPlayer(player);
            if (paintballMap == null) return;

            PaintballGamePlayer paintballGamePlayer = paintballMap.GetPlayerById(player.SqlId);
            if (paintballGamePlayer == null) return;

            paintballGamePlayer.Deaths += 1;
            paintballGamePlayer.Killstreak = 0;

            if (killer == null || killer.Minigame != MinigameTypes.PAINTBALL) return;
            PaintballGamePlayer paintballGameKiller = paintballMap.GetPlayerById(killer.SqlId);
            paintballGameKiller.Kills += 1;
            paintballGameKiller.Killstreak += 1;

            if (VerifyGameWin(paintballGameKiller))
                FinishGame(paintballMap, paintballGameKiller);

            await Task.CompletedTask;
        }

        public Task<bool> HasSpecialInjury(PXPlayer player)
        {
            if (player.Minigame != MinigameTypes.PAINTBALL) return Task.FromResult(false);
            if (player.DimensionType != DimensionTypes.PAINTBALL) return Task.FromResult(false);

            return Task.FromResult(true);
        }

        public async Task ApplyInjury(PXPlayer player)
        {
            if (player.Minigame != MinigameTypes.PAINTBALL) return;

            await Task.Delay(4000);
            if (new AsyncPlayerRef(player).Exists && player.IsValid())
            {
                await player.Revive();

                var paintballMap = PaintballModule.Instance.GetPaintballGameMapByPlayer(player);
                if (paintballMap == null) return;

                await player.SetPositionAsync(paintballMap.Data.Spawns.PickRandom().Position);
                await player.SetArmorAsync(player.MaxArmor);
            }
        }
    }
}
