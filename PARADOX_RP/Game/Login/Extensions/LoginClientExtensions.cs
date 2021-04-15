using AltV.Net.Async;
using AltV.Net.Data;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Game.Commands.Extensions;
using PARADOX_RP.UI;
using PARADOX_RP.UI.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Login.Extensions
{
    static class LoginClientExtensions
    {
        public static async Task PreparePlayer(this PXPlayer client, Position pos) {
            await client.SpawnAsync(pos);

            await Task.Delay(2000);
            WindowManager.Instance.Get<HUDWindow>().Show(client, new HUDWindowWriter(client.SqlId, client.Username, client.Money));
            await Task.Delay(2000);
            client.SendChatMessage("PARADOX RP", "PreparePlayer", true);
            client.SendNotification("PARADOX RP", "PreparePlayer", NotificationTypes.SUCCESS);
        }
    }
}
