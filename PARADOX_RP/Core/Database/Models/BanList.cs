using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    class BanList
    {
        public BanList(int playerId, int moderatorId, bool active, DateTime createdAt)
        {
            PlayerId = playerId;
            ModeratorId = moderatorId;
            Active = active;
            CreatedAt = createdAt;
        }

        public BanList(int playerId, string description, bool active, DateTime createdAt)
        { 
            // SYSTEM - BAN
            
            PlayerId = playerId;
            ModeratorId = 1;
            Description = description;
            Active = active;
            CreatedAt = createdAt;
        }

        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int ModeratorId { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Players Player { get; set; }
        public virtual Players Moderator { get; set; }
    }
}
