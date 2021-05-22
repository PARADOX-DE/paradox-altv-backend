using AltV.Net.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    [Table("vehicle_shops")]
    public partial class VehicleShops
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Position_X { get; set; }
        public float Position_Y { get; set; }
        public float Position_Z { get; set; }

        public virtual ICollection<VehicleShopsContent> Content { get; set; }

        public float BoughtPosition_X { get; set; }
        public float BoughtPosition_Y { get; set; }
        public float BoughtPosition_Z { get; set; }

        public float BoughtRotation_X { get; set; }
        public float BoughtRotation_Y { get; set; }
        public float BoughtRotation_Z { get; set; }
    }

    public partial class VehicleShops
    {
        public Position Position => new Position(Position_X, Position_Y, Position_Z);
        public Position BoughtPosition => new Position(BoughtPosition_X, BoughtPosition_Y, BoughtPosition_Z);
        public Rotation BoughtRotation => new Rotation(BoughtRotation_X, BoughtRotation_Y, BoughtRotation_Z);
    }
}
