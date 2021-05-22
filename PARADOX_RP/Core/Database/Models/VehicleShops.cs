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
    }

    public partial class VehicleShops
    {
        public Position Position => new Position(Position_X, Position_Y, Position_Z);
    }
}
