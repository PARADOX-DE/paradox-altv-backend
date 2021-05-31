using AltV.Net.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    public partial class Jumppoints
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TeamId { get; set; }
        public bool Locked { get; set; }
        public bool Vehicle { get; set; } = false;
        public DateTime LastBreaked { get; set; }
        public int Dimension { get; set; }
        public int EndDimension { get; set; }
        public float Position_X { get; set; }
        public float Position_Y { get; set; }
        public float Position_Z { get; set; }
        public float Rotation_X { get; set; }
        public float Rotation_Y { get; set; }
        public float Rotation_Z { get; set; }
        public float End_Position_X { get; set; }
        public float End_Position_Y { get; set; }
        public float End_Position_Z { get; set; }
        public float End_Rotation_X { get; set; }
        public float End_Rotation_Y { get; set; }
        public float End_Rotation_Z { get; set; }
    }

    public partial class Jumppoints
    {
        public Position Position => new Position(Position_X, Position_Y, Position_Z);
        public Rotation Rotation => new Rotation(Rotation_X, Rotation_Y, Rotation_Z);
        public Position EndPosition => new Position(End_Position_X, End_Position_Y, End_Position_Z);
        public Rotation EndRotation => new Rotation(End_Rotation_X, End_Rotation_Y, End_Rotation_Z);
    }
}
