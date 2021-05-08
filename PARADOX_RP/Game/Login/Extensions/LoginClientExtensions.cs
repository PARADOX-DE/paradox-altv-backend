using AltV.Net.Async;
using AltV.Net.Data;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Game.Commands.Extensions;
using PARADOX_RP.Game.Inventory.Extensions;
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
        public static async Task PreparePlayer(this PXPlayer client, Position pos)
        {
            if (!await client.ExistsAsync()) return;
            
            await client.SpawnAsync(pos);
            WindowManager.Instance.Get<HUDWindow>().Show(client, new HUDWindowWriter(client.SqlId, client.Username, client.Money));

            /*
             * SHOW VOICE-SECTION
             */
            AltAsync.Log(client.Inventory.HasItem(1).ToString());

            if (client.Inventory.HasItem(1))
            {
                client.HasPhone = true;
                AltAsync.Log("Has phone");
            }
        }
    }
}
