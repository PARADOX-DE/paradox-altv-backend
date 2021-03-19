using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    [Table("player_clothes")]
    class PlayerClothes
    {
        public int Id { get; set; }
        public virtual Players Player { get; set; }

        public Clothes Mask { get; set; }
        public Clothes Torso { get; set; }
        public Clothes Legs { get; set; }
        public Clothes Bag { get; set; }
        public Clothes Shoe { get; set; }
        public Clothes Accessoire { get; set; }
        public Clothes Undershirt { get; set; }
        public Clothes Armor { get; set; }
        public Clothes Decal { get; set; }
        public Clothes Top { get; set; }
    }
}
