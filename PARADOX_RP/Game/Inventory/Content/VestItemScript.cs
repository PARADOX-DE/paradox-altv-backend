using AltV.Net.Async;
using AltV.Net.Async.Elements.Refs;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Game.Inventory.Interfaces;
using PARADOX_RP.Game.Misc.Progressbar.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Inventory.Content
{
    class VestItemScript : IItemScript
    {
        public string ScriptName => "vest_itemscript";

        private const int VEST_DURATION = 4 * 1000;

        public async Task<bool> UseItem(PXPlayer player)
        {
            await player.PlayAnimation("anim@heists@narcotics@funding@gang_idle", "gang_chatting_idle01");

            bool finishedProgressbar = await player.RunProgressBar(async () =>
            {
                await player.SetArmorAsync(100);
            }, "Schutzweste", "Du ziehst nun eine Schutzweste...", VEST_DURATION);

            if (new AsyncPlayerRef(player).Exists)
                await player.StopAnimation();

            return finishedProgressbar;
        }
    }
}
