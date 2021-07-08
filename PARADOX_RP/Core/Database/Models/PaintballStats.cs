using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    [Table("paintball_stats")]
    public class PaintballStats
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }

        public int LeaguePoints { get; set; }
        public int RankId { get; set; }

        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Killstreak { get; set; }

        public virtual Players Player { get; set; }
        public virtual PaintballRanks Rank { get; set; }
    }
}
