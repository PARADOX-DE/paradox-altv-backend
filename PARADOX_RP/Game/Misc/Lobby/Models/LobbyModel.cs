using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Lobby
{
    public enum LobbyStatus
    {
        WAITING,
        INGAME,
        END
    }

    public class LobbyModel
    {
        public int OwnerId { get; set; }
        public string Owner { get; set; }
        public int MaxCounts { get; set; }

        public LobbyStatus Status { get; set; }
    }
}
