using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Clothing.Models
{
    public class ShopClothModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ComponentVariation ComponentVariation { get; set; }
        public Dictionary<int, Clothes> Variants { get; set; }
        public int Price { get; set; }
        public Gender Gender { get; set; }
    }
}
