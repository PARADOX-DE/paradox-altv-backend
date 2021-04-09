using PARADOX_RP.Game.Inventory;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    public partial class Inventories
    {
        public Inventories()
        {
            Items = new HashSet<InventoryItemAssignments>();
        }

        public int Id { get; set; }
        public int TargetId { get; set; }
        public InventoryTypes Type { get; set; }
        public virtual ICollection<InventoryItemAssignments> Items { get; set; }
    }
}
