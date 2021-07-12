using AltV.Net.Async;
using AltV.Net.Async.Elements.Refs;
using AltV.Net.Data;
using EntityStreamer;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Game.Inventory.Interfaces;
using PARADOX_RP.Game.Misc.Progressbar.Extensions;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Inventory.Content
{
    class CoffeeItemScript : IItemScript
    {
        public string ScriptName => "coffee_itemscript";

        public async Task<bool> UseItem(PXPlayer player)
        {
            await player.PlayAnimation("amb@world_human_drinking@coffee@male@idle_a", "idle_a");

            await Task.Delay(10000);
            if (new AsyncPlayerRef(player).Exists)
            {
                await player.StopAnimation();
                player.SendNotification("Kaffee", "Du hast einen leckeren Kaffee getrunken!", NotificationTypes.SUCCESS);
            }

            return await Task.FromResult(true);
        }
    }
}