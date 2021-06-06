using AltV.Net.Data;
using PARADOX_RP.Game.Inventory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    [Table("shop_items")]
    public partial class ShopItems
    {
        public int Id { get; set; }
        public int ShopId { get; set; }
        public int ItemId { get; set; }
        public int Price { get; set; }

        public virtual Shops Shop { get; set; }
        public virtual Items Item { get; set; }
    }
}
