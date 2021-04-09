using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    [Table("inventory_item_signatures")]
    public class InventoryItemSignatures
    {
        public int Id { get; set; }
        public string Origin { get; set; }
        public string Information { get; set; }
    }
}
