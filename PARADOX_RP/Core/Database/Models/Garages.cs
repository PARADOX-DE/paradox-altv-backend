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


        public float Spawn_Position_X { get; set; }
        public float Spawn_Position_Y { get; set; }
        public float Spawn_Position_Z { get; set; }

        public float Spawn_Rotation_X { get; set; }
        public float Spawn_Rotation_Y { get; set; }
        public float Spawn_Rotation_Z { get; set; }

        /*
         * 
         * TODO: SPLIT SPAWNS INTO ANOTHER DB MODELS
         * 
         */

        public virtual Teams Team { get; set; }
    }

    public partial class Garages
    {
        public Position Position => new Position(Position_X, Position_Y, Position_Z);
        public Position Spawn_Position => new Position(Spawn_Position_X, Spawn_Position_Y, Spawn_Position_Z);
        public Rotation Spawn_Rotation => new Rotation(Spawn_Rotation_X, Spawn_Rotation_Y, Spawn_Rotation_Z);
    }
}
