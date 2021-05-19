using AltV.Net;
using AltV.Net.Elements.Entities;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Misc.AntiCombatLog
{
    class AntiCombatLogModule : ModuleBase<AntiCombatLogModule>
    {
        public AntiCombatLogModule() : base("AntiCombatLog") { }

        public override void OnPlayerDisconnect(PXPlayer player)
        {
            if (!player.LoggedIn) return;

            foreach (PXPlayer pxPlayer in Pools.Instance.Get<PXPlayer>(PoolType.PLAYER))
            {
                if (pxPlayer.DimensionType == DimensionTypes.WORLD)
                    pxPlayer.SendNotification("Offlineflucht", $"Der Spieler {player.Username} hat die Verbindung getrennt", NotificationTypes.SUCCESS);
            }
        }
    }
}