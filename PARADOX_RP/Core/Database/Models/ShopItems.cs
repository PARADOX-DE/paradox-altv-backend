using AltV.Net.Data;
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
        public int ShopsId { get; set; }
        public int Item { get; set; }
        public int Price { get; set; }
    }

    public partial class ShopItems
    {

    }
}
