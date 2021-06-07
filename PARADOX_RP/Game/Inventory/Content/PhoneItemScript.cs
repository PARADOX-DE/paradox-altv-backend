using AltV.Net.Async;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Game.Inventory.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Inventory.Content
{
    class PhoneItemScript : IItemScript
    {
        public string ScriptName => "phone_itemscript";

        public Task<bool> UseItem(PXPlayer player) 
        {
            player.HasPhone = !player.HasPhone;
            if (player.HasPhone)
                player.SendNotification("Smartphone", "Du hast dein Smartphone angeschalten.", NotificationTypes.SUCCESS);
            else
                player.SendNotification("Smartphone", "Du hast dein Smartphone ausgeschalten.", NotificationTypes.SUCCESS);

            return Task.FromResult(false);
        }
    }
}
