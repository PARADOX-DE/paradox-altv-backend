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

        public BanList Mask { get; set; }
        public BanList Torso { get; set; }
        public BanList Legs { get; set; }
        public BanList Bag { get; set; }
        public BanList Shoe { get; set; }
        public BanList Accessoire { get; set; }
        public BanList Undershirt { get; set; }
        public BanList Armor { get; set; }
        public BanList Decal { get; set; }
        public BanList Top { get; set; }
    }
}
