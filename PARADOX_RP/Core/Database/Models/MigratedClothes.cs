using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    public class MigratedClothes
    {
        public MigratedClothes()
        {
            Variants = new HashSet<MigratedClothesVariants>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public Gender Gender { get; set; }
        public ComponentVariation ComponentVariation { get; set; }
        public int Price { get; set; }

        public int ClothesShopId { get; set; }
        public virtual ClothesShop ClothesShop { get; set; }
        public virtual ICollection<MigratedClothesVariants> Variants { get; set; }
    }
}
