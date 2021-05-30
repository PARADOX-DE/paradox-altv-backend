using AltV.Net.Data;
using PARADOX_RP.Game.Inventory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    public partial class GasStations
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OwnerId { get; set; } = -1;
        public int TankVolume { get; set; }
        public int Price { get; set; }
        public float Position_X { get; set; }
        public float Position_Y { get; set; }
        public float Position_Z { get; set; }
        public int Petrol { get; set; }
        public int Diesel { get; set; }
        public int Electro { get; set; }
    }

    public partial class GasStations
    {
        public Position Position => new Position(Position_X, Position_Y, Position_Z);
    }
}
