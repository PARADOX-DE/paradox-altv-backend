using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    public class Vehicles
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

        public DateTime CreatedAt { get; set; }

        public virtual Players Player { get; set; }
        public virtual Garages Garage { get; set; }
    }
}
