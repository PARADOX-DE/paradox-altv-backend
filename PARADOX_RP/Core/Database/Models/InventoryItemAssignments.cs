using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    [Table("inventory_item_assignments")]
    public class InventoryItemAssignments
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public int OriginId { get; set; }

        public virtual Inventories Inventory { get; set; }
        public virtual InventoryItemSignatures Origin { get; set; }

        public int Item { get; set; }
        public string Attribute { get; set; }
        public float Weight { get; set; }
        public int Slot { get; set; }
    }
}
