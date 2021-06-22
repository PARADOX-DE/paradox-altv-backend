using AltV.Net.Data;
using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    public partial class JumpPoints
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Locked { get; set; }

        public bool VehicleAccess { get; set; }

        public bool Breakable { get; set; }
        public DateTime LastBreaked { get; set; }
        public int DestinationId { get; set; }

        public int Dimension { get; set; }
        public DimensionTypes DimensionType { get; set; }

        public float Position_X { get; set; }
        public float Position_Y { get; set; }
        public float Position_Z { get; set; }

        public float Rotation_X { get; set; }
        public float Rotation_Y { get; set; }
        public float Rotation_Z { get; set; }

        public float Range { get; set; } = 3f;
        
        public virtual JumpPoints Destination { get; set; }
        public virtual ICollection<JumpPointPermissions> Permissions { get; set; }
    }

    public partial class JumpPoints
    {
        public Position Position => new Position(Position_X, Position_Y, Position_Z);
        public Rotation Rotation => new Rotation(Rotation_X, Rotation_Y, Rotation_Z);
    }
}
