using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    [Table("player_customization")]
    public class PlayerCustomization
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }

        public string Customization { get; set; }

        public virtual Players Player { get; set; }
    }
}
