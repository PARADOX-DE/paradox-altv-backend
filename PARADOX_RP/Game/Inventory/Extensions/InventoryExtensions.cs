using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Game.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Inventory.Extensions
{
    public static class InventoryExtensions
    {
        public static bool HasItem(this PXInventory inventory, int ItemId) => InventoryModule.Instance.HasItem(inventory, ItemId);

        public static Task<bool?> CanAccess(this PXInventory inventory, PXPlayer player) => InventoryModule.Instance.CanAccessInventory(inventory, player);
    }
}
