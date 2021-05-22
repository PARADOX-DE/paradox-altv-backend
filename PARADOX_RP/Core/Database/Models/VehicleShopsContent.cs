using AltV.Net.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    [Table("vehicle_shops_content")]
    public partial class VehicleShopsContent
    {
        public int Id { get; set; }
        public string VehicleModel { get; set; }
        public int Price { get; set; }

        public float PreviewPosition_X { get; set; }
        public float PreviewPosition_Y { get; set; }
        public float PreviewPosition_Z { get; set; }

        public float PreviewRotation_X { get; set; }
        public float PreviewRotation_Y { get; set; }
        public float PreviewRotation_Z { get; set; }
    }

    public partial class VehicleShopsContent
    {
        public Position PreviewPosition => new Position(PreviewPosition_X, PreviewPosition_Y, PreviewPosition_Z);
        public Rotation PreviewRotation => new Rotation(PreviewRotation_X, PreviewRotation_Y, PreviewRotation_Z);
    }
}
