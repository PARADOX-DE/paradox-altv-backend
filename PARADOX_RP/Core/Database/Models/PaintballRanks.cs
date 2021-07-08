using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    [Table("paintball_ranks")]
    public class PaintballRanks
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        public int MinLeaguePoints { get; set; }
        public int MaxLeaguePoints { get; set; }
    }
}
