using PARADOX_RP.Game.Administration.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    public class Players
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string HardwareId { get; set; }
        public string DiscordId { get; set; }
        public string SocialClubName { get; set; }
        public string SocialClubHash { get; set; }

        public virtual SupportRankModel SupportRank { get; set; }
        //public virtual PlayerClothes PlayerClothes { get; set; }

        public DateTime LastLogin { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
