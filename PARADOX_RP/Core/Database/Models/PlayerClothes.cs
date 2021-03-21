using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    [Table("player_clothes")]
    public class PlayerClothes
    {
        public int Id { get; set; }
        public virtual Players Player { get; set; }

        public virtual Clothes Mask { get; set; }
        public virtual Clothes Torso { get; set; }
        public virtual Clothes Legs { get; set; }
        public virtual Clothes Bag { get; set; }
        public virtual Clothes Shoe { get; set; }
        public virtual Clothes Accessoire { get; set; }
        public virtual Clothes Undershirt { get; set; }
        public virtual Clothes Armor { get; set; }
        public virtual Clothes Decal { get; set; }
        public virtual Clothes Top { get; set; }
    }
}
