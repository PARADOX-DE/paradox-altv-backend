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
    class MedkitItemScript : IItemScript
    {
        public string ScriptName => "medkit_itemscript";
        private const int MEDKIT_DURATION = 3 * 1000;
        public async Task<bool> UseItem(PXPlayer player)
        {

            await player.PlayAnimation("anim@heists@narcotics@funding@gang_idle", "gang_chatting_idle01");

            bool finishedProgressbar = await player.RunProgressBar(async () =>
            {
                await player.SetHealthAsync(player.MaxHealth);
            }, "Verbandskasten", "Du versorgst nun deine Wunden.", MEDKIT_DURATION);

            if (new AsyncPlayerRef(player).Exists)
                await player.StopAnimation();

            return finishedProgressbar;
        }
    }
}
