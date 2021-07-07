using PARADOX_RP.Core.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Inventory.Models
{
    public class PXInventory
    {
        public int Id { get; set; }
        public InventoryInfo InventoryInfo { get; set; }
        public int TargetId { get; set; }
        public Dictionary<int, InventoryItemAssignments> Items { get; set; }
        public bool Locked { get; set; }

        public PXInventory()
        {
            Items = new Dictionary<int, InventoryItemAssignments>();
            TargetId = -1;
            InventoryInfo = null;
            Locked = false;
        }
    }
}
