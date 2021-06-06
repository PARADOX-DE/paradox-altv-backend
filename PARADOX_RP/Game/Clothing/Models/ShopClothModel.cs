using PARADOX_RP.Core.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Clothing.Models
{
    class ShopClothModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Dictionary<int, Clothes> Variants { get; set; }
        public int Price { get; set; }
    }
}
