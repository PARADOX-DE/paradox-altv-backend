using AltV.Net.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    [Table("position_list")]
    public partial class PositionList
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public float Position_X { get; set; }
        public float Position_Y { get; set; }
        public float Position_Z { get; set; }
    }

    public partial class PositionList
    {
        public Position Position => new Position(Position_X, Position_Y, Position_Z);
    }
}
