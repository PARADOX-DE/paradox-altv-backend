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
        public FuelTypes FuelType { get; set; } = FuelTypes.PETROL;

        private float _fuel;
        public float Fuel
        {
            get => _fuel;
            set
            {
                this.SetStreamSyncedMetaDataAsync("Fuel", _fuel);
                _fuel = value;
            }
        }

        private bool _hasRadio;
        public bool HasRadio
        {
            get => _hasRadio;
            set
            {
                this.SetStreamSyncedMetaDataAsync("HasRadio", value);
                _hasRadio = value;
            }
        }

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

        private bool _engine;
        public bool Engine
        {
            get => _engine;
            set
            {
                this.SetEngineOnAsync(value);
                _engine = value;
            }
        }

        internal PXVehicle(IntPtr nativePointer, ushort id) : base(nativePointer, id)
        {
            SqlId = -1;
            VehicleModel = "Unknown";
            HasRadio = false;
            Inventory = null;
            Fuel = 50;
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
