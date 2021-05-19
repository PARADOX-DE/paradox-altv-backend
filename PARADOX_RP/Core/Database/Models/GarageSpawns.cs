using AltV.Net.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    [Table("garage_spawns")]
    public partial class GarageSpawns
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int GarageId { get; set; }
        public virtual Garages Garage { get; set; }

        public float Spawn_Position_X { get; set; }
        public float Spawn_Position_Y { get; set; }
        public float Spawn_Position_Z { get; set; }

        public float Spawn_Rotation_X { get; set; }
        public float Spawn_Rotation_Y { get; set; }
        public float Spawn_Rotation_Z { get; set; }
    }

    public partial class GarageSpawns
    {
        public Position Spawn_Position => new Position(Spawn_Position_X, Spawn_Position_Y, Spawn_Position_Z);
        public Rotation Spawn_Rotation => new Rotation(Spawn_Rotation_X, Spawn_Rotation_Y, Spawn_Rotation_Z);
    }
}
