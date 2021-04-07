using AltV.Net.Data;
using PARADOX_RP.Game.Inventory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    [Table("garages")]
    public partial class Garages
    {
        public Garages()
        {
            Vehicles = new HashSet<Vehicles>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int TeamId { get; set; }
        public virtual ICollection<Vehicles> Vehicles { get; set; }

        public float Position_X { get; set; }
        public float Position_Y { get; set; }
        public float Position_Z { get; set; }

        public virtual Teams Team { get; set; }
    }

    public partial class Garages
    {
        public Position Position => new Position(Position_X, Position_Y, Position_Z);
    }
}
