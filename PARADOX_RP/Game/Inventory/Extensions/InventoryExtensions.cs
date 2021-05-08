using PARADOX_RP.Core.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Inventory.Extensions
{
    public static class InventoryExtensions
    {
        public static bool HasItem(this Inventories inventory, int ItemId) => InventoryModule.Instance.HasItem(inventory, ItemId);
    }
}
