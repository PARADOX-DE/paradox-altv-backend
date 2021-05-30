using AltV.Net.Data;
using PARADOX_RP.Game.Inventory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    [Table("gasstation_petrols")]
    public partial class GasStationPetrols
    {
        public int Id { get; set; }
        public int GasStationId { get; set; }
        public float Position_X { get; set; }
        public float Position_Y { get; set; }
        public float Position_Z { get; set; }

        public virtual GasStations GasStation { get; set; }
    }

    public partial class GasStationPetrols
    {
        public Position Position => new Position(Position_X, Position_Y, Position_Z);
    }
}
