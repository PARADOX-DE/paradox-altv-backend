using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    [Table("player_injury_data")]
    public class PlayerInjuryData
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }

        public int InjuryId { get; set; }
        public int InjuryTimeLeft { get; set; }

        public virtual Players Player { get; set; }
    }
}
