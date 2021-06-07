using AltV.Net.Data;
using PARADOX_RP.Game.Administration.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    public partial class Players
    {
        public Players()
        {
            PlayerCustomization = new HashSet<PlayerCustomization>();
            PlayerClothes = new HashSet<PlayerClothesWearing>();
            PlayerTeamData = new HashSet<PlayerTeamData>();
            PlayerInjuryData = new HashSet<PlayerInjuryData>();
            PlayerWeapons = new HashSet<PlayerWeapons>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string HardwareId { get; set; }
        public string DiscordId { get; set; }
        public string SocialClubName { get; set; }
        public string SocialClubHash { get; set; }
        public bool Arrived { get; set; }
        public int Money { get; set; }
        public int BankMoney { get; set; }
        public int TeamsId { get; set; }

        /* SPAWN POS */
        public float Position_X { get; set; }
        public float Position_Y { get; set; }
        public float Position_Z { get; set; }


        public virtual SupportRankModel SupportRank { get; set; }
        public virtual Teams Team { get; set; }
        public virtual ICollection<PlayerCustomization> PlayerCustomization { get; set; }
        public virtual ICollection<PlayerClothesWearing> PlayerClothes { get; set; }
        public virtual ICollection<PlayerWeapons> PlayerWeapons { get; set; }
        public virtual ICollection<PlayerTeamData> PlayerTeamData { get; set; }
        public virtual ICollection<PlayerInjuryData> PlayerInjuryData { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public partial class Players
    {
        public Position Position => new Position(Position_X, Position_Y, Position_Z);

    }
}
