using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    [Table("player_clothes")]
    public class PlayerClothesWearing
    {

        public int Id { get; set; }

        public int PlayerId { get; set; }
        public ComponentVariation ComponentVariation { get; set; }
        public int ClothingId { get; set; }

        public virtual Players Player { get; set; }
        public virtual Clothes Clothing { get; set; }
    }
}
