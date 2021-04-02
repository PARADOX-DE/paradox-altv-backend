using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    [Table("player_team_data")]
    public class PlayerTeamData
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int Rank { get; set; }
        public int Payday { get; set; }
        public DateTime Joined { get; set; }

        public virtual Players Player { get; set; }
    }
}
