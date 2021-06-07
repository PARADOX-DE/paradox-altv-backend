using AltV.Net.Async;
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

        public async Task<bool> UseItem()
        {

            return false;
        }
    }
}
