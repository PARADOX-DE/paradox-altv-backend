using AltV.Net.Elements.Entities;
using EntityStreamer;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Game.Lobby;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Paintball.Models
{
    public class PaintballGameMap
    {
        public PaintballMaps Data { get; set; }
        public LobbyStatus LobbyStatus { get; set; } = LobbyStatus.WAITING;

        public Dictionary<int, PaintballGamePlayer> Players { get; set; } = new Dictionary<int, PaintballGamePlayer>(); // <= int = SqlId

        public List<IColShape> ColShapes { get; set; } = new List<IColShape>();
        public List<Marker> Markers { get; set; } = new List<Marker>();

        public PaintballGameMap(PaintballMaps data)
        {
            Data = data;
        }

        public PaintballGamePlayer GetPlayerById(int SqlId)
        {
            if (Players.TryGetValue(SqlId, out PaintballGamePlayer player))
                return player;

            return null;
        }
    }
}
