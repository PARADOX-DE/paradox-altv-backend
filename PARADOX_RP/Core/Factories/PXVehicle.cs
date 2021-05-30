using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Game.Inventory.Models;
using PARADOX_RP.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Factories
{
    public enum FuelTypes
    {
        PETROL,
        DIESEL,
        ELECTRO
    }
    public class PXVehicle : Vehicle
    {
        public int SqlId { get; set; }
        public string VehicleModel { get; set; }
        public int OwnerId { get; set; }
        public bool HasRadio { get; set; }
        public FuelTypes FuelType { get; set; } = FuelTypes.PETROL;
        public float Fuel { get; set; }
        public PXInventory Inventory { get; set; }

        private bool _locked;
        public bool Locked
        {
            get => _locked;
            set
            {
                this.SetLockStateAsync(value ? VehicleLockState.Locked : VehicleLockState.Unlocked);
                _locked = value;
            }
        }


        internal PXVehicle(IntPtr nativePointer, ushort id) : base(nativePointer, id)
        {
            SqlId = -1;
            VehicleModel = "Unknown";
            HasRadio = false;
            Inventory = null;
        }

        public bool IsValid()
        {
            return SqlId > 0;
        }
    }

    internal class PXVehicleFactory : IEntityFactory<IVehicle>
    {
        public IVehicle Create(IntPtr entityPointer, ushort id)
        {
            return new PXVehicle(entityPointer, id);
        }
    }
}
