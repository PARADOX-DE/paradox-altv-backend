using AltV.Net.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    public partial class CryptoRooms
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Position_X { get; set; }
        public float Position_Y { get; set; }
        public float Position_Z { get; set; }

        public int Servers { get; set; }
        public int OwnerId { get; set; }

        public virtual Players Owner { get; set; }
    }

    public partial class CryptoRooms
    {
        public Position Position => new Position(Position_X, Position_Y, Position_Z);
        public bool Locked { get; set; } = false;
    }
}
