using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Inventory.Extensions
{
    public static class InventoryExtensions
    {
        public static bool HasItem(this Inventories inventory, int ItemId) => InventoryModule.Instance.HasItem(inventory, ItemId);

        public static Task<bool?> CanAccess(this Inventories inventory, PXPlayer player) => InventoryModule.Instance.CanAccessInventory(inventory, player);
    }
}
