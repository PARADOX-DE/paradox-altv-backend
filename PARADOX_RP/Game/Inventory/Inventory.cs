using PARADOX_RP.Core.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Inventory
{
    public class Inventory
    {
        public int SqlId { get; set; }
        public virtual ICollection<InventoryItemAssignments> Items { get; set; }

    }
}
