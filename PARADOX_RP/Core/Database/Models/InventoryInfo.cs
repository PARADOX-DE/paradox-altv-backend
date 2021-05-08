using PARADOX_RP.Game.Inventory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    [Table("inventory_info")]
    public class InventoryInfo
    {
        public int Id { get; set; }
        public InventoryTypes InventoryType { get; set; }
        public string Name { get; set; }
        public int MaxSlots { get; set; }
        public int MaxWeight { get; set; }
    }
}
