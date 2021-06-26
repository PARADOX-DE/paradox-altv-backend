using AltV.Net.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    [Table("clothes_shop")]
    public partial class ClothesShop
    {
        public ClothesShop()
        {
            Clothes = new HashSet<MigratedClothes>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int TeamId { get; set; }

        public float Position_X { get; set; }
        public float Position_Y { get; set; }
        public float Position_Z { get; set; }

        public virtual ICollection<MigratedClothes> Clothes { get; set; }
        public virtual Teams Team { get; set; }
    }

    public partial class ClothesShop
    {
        public Position Position => new Position(Position_X, Position_Y, Position_Z);
    }
}
