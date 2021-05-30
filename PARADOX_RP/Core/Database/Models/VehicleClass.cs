using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    [Table("vehicle_class")]
    public class VehicleClass
    {

        public VehicleClass()
        {
            Vehicle = new HashSet<Vehicles>();
            VehicleShopsContent = new HashSet<VehicleShopsContent>();
        }

        public int Id { get; set; }
        public string VehicleModel { get; set; }


        public float MaxSpeed { get; set; }
        public float MaxFuel { get; set; }
        public float FuelConsumption { get; set; }
        public FuelTypes FuelType { get; set; }


        /* TRUNK */
        public int InvWeight { get; set; } // IN GRAMM | tashish
        public int InvSlots { get; set; }

        public byte Seats { get; set; }

        public virtual ICollection<Vehicles> Vehicle { get; set; }
        public virtual ICollection<VehicleShopsContent> VehicleShopsContent { get; set; }
    }
}
