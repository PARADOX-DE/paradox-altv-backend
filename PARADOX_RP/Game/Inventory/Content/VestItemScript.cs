using AltV.Net.Async;
using PARADOX_RP.Game.Inventory.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Inventory.Content
{
    class VestItemScript : IItemScript
    {
        public string ScriptName => "vest_itemscript";

        public int Id => 1;

        public async Task<bool> UseItem()
        {
            AltAsync.Log("Used Item Vest");
            return true;
        }
    }
}
