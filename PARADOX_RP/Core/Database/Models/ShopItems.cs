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
        public int ShopsId { get; set; }
        public int Item { get; set; }
        public int Price { get; set; }
    }

    public partial class ShopItems
    {
        public ShopItems () {
            if (!InventoryModule.Instance._items.TryGetValue(this.Item, out Items Item)) return;

            Name = Item.Name;
        }
        public string Name { get; set; }
    }
}
