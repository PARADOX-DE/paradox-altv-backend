using PARADOX_RP.Core.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Lobby
{
    class LobbyModule : ModuleBase<LobbyModule>
    {
        public LobbyModule() : base("Lobby") { }

        public int GetDimensionByLobby(LobbyModel lobby)
        {
            return lobby.GetHashCode(); 
        }

    }
}
