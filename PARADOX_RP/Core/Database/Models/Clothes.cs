using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    public class Clothes
    {
        public Clothes()
        {
            Variants = new HashSet<ClothesVariants>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public Gender Gender { get; set; }
        public ComponentVariation ComponentVariation { get; set; }
        public int Price { get; set; }

        public int ClothesShopId { get; set; }
        public virtual ClothesShop ClothesShop { get; set; }
        public virtual ICollection<ClothesVariants> Variants { get; set; }
    }
}
