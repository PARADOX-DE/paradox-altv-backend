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
        public List<PXPlayer> Players { get; set; } = new List<PXPlayer>();

        public List<IColShape> ColShapes { get; set; } = new List<IColShape>();
        public List<Marker> Markers { get; set; } = new List<Marker>();

        public PaintballGameMap(PaintballMaps data)
        {
            Data = data;
        }
    }
}
