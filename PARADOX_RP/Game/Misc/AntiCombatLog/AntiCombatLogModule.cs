using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Async.Elements.Refs;
using AltV.Net.Elements.Entities;
using PARADOX_RP.Core.Events;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Misc.AntiCombatLog
{
    class AntiCombatLogModule : ModuleBase<AntiCombatLogModule>, IEventPlayerDisconnect
    {
        public AntiCombatLogModule() : base("AntiCombatLog") { }

        public async void OnPlayerDisconnect(PXPlayer player)
        {
            if (!await player.ExistsAsync()) return;
            if (!player.LoggedIn) return;

            foreach (PXPlayer pxPlayer in Pools.Instance.Get<PXPlayer>(PoolType.PLAYER))
            {
                lock (pxPlayer)
                {
                    if (!pxPlayer.Exists) continue;
                }

                if ((await pxPlayer.GetPositionAsync()).Distance((await player.GetPositionAsync())) <= 20)
                    if (pxPlayer.DimensionType == DimensionTypes.WORLD)
                        pxPlayer.SendNotification("Offlineflucht", $"Der Spieler {player.Username} hat die Verbindung getrennt", NotificationTypes.SUCCESS);
            }
        }
    }
}