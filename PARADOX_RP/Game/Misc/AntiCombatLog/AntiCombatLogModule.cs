using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Async.Elements.Refs;
using AltV.Net.Elements.Entities;
using PARADOX_RP.Core.Events;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Utils;
using PARADOX_RP.Utils.Callbacks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Misc.AntiCombatLog
{
    class AntiCombatLogModule : ModuleBase<AntiCombatLogModule>, IEventPlayerDisconnect
    {
        public AntiCombatLogModule() : base("AntiCombatLog") { }

        public async void OnPlayerDisconnect(PXPlayer player)
        {
            if (!player.LoggedIn) return;

            await Alt.ForEachPlayers(new AsyncFunctionCallback<IPlayer>(async (basePlayer) =>
            {
                if (!(basePlayer is PXPlayer pxPlayer))
                {
                    return;
                }

                var playerPos = AltV.Net.Data.Position.Zero; pxPlayer.GetPositionLocked(ref playerPos);
                var targetPos = AltV.Net.Data.Position.Zero; player.GetPositionLocked(ref playerPos);

                if (playerPos.Distance(targetPos) <= 20)
                    if (pxPlayer.DimensionType == DimensionTypes.WORLD)
                        pxPlayer.SendNotification("Offlineflucht", $"Der Spieler {player.Username} hat die Verbindung getrennt", NotificationTypes.SUCCESS);

                await Task.CompletedTask;
            }));
        }
    }
}