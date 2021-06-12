using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Lobby
{
    class LobbyModule : ModuleBase<LobbyModule>
    {
        public Dictionary<int, LobbyModel> _lobbys = new Dictionary<int, LobbyModel>();
        public LobbyModule() : base("Lobby") { }

        public LobbyModel RegisterLobby(PXPlayer player, int MaxCounts)
        {
            LobbyModel lobby = new LobbyModel() { OwnerId = player.SqlId, MaxCounts = MaxCounts, Owner = player.Username };
            bool result = _lobbys.TryAdd(player.SqlId, lobby);
            
            if (result) return lobby;
            else return null;
        }

        public int GetDimensionByLobby(LobbyModel lobby)
        {
            return lobby.OwnerId;
        }
    }
}
