using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    [Table("paintball_settings")]
    public class PaintballSettings
    {
        public int Id { get; set; }
        public int MAX_ROUND_PLAYERS { get; set; }
        public int MIN_PLAYERS_START_ROUND { get; set; }
        public int DAILY_LOST_LEAGUE_POINTS { get; set; }
        public DateTime LastPointDrop { get; set; } = DateTime.Now;
    }
}
