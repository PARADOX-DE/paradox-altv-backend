using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    public partial class Vehicles
    {
        public Vehicles(int playerId, string vehicleModel)
        {
            PlayerId = playerId;
            VehicleModel = vehicleModel;
        }

        public int Id { get; set; }
        public int PlayerId { get; set; }
        public string VehicleModel { get; set; }
        public int GarageId { get; set; }
        public bool Parked { get; set; }

        public float Position_X { get; set; }
        public float Position_Y { get; set; }
        public float Position_Z { get; set; }
        public float Rotation_R { get; set; }
        public float Rotation_P { get; set; }
        public float Rotation_Y { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual Players Player { get; set; }
        public virtual Garages Garage { get; set; }
    }

    public partial class Vehicles
    {
        public Position Position => new Position(Position_X, Position_Y, Position_Z);
        public Rotation Rotation => new Rotation(Rotation_R, Rotation_P, Rotation_Y);
    }
}
