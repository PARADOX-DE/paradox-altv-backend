using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Inventory;
using PARADOX_RP.Game.Inventory.Interfaces;
using PARADOX_RP.Game.Inventory.Models;
using PARADOX_RP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Vehicle
{
    class VehicleModule : ModuleBase<VehicleModule>, IInventoriable
    {
        public readonly Dictionary<int, VehicleClass> _vehicleClass = new Dictionary<int, VehicleClass>();
        public VehicleModule(PXContext pxContext) : base("Vehicle")
        {
            LoadDatabaseTable<VehicleClass>(pxContext.VehicleClass, (v) =>
            {
                _vehicleClass.Add(v.Id, v);
            });
        }

        public Task<PXInventory> OnInventoryOpen(PXPlayer player, Position position)
        {
            PXVehicle vehicle = Pools.Instance.Get<PXVehicle>(PoolType.VEHICLE).FirstOrDefault(v => v.Position.Distance(position) < 2.5);
            if (vehicle != null)
            {
                if (Configuration.Instance.DevMode)
                {
                    AltAsync.Log("Inventory found: " + vehicle.Inventory.InventoryInfo.InventoryType + " " + vehicle.Inventory.Id);
                }

                return Task.FromResult(vehicle.Inventory);
            }

            return Task.FromResult<PXInventory>(null);
        }

        public Task<bool?> CanAccessInventory(PXPlayer player, PXInventory inventory)
        {
            if (inventory.InventoryInfo.InventoryType != InventoryTypes.VEHICLE) return Task.FromResult<bool?>(null);

            PXVehicle vehicle = Pools.Instance.Get<PXVehicle>(PoolType.VEHICLE).FirstOrDefault(v => v.SqlId == inventory.TargetId);
            if (vehicle != null)
            {
                if (Configuration.Instance.DevMode)
                {
                    AltAsync.Log("Inventory can access: " + vehicle.Inventory.InventoryInfo.InventoryType + " " + vehicle.Inventory.Id);
                }

                return Task.FromResult<bool?>(true);
            }

            return Task.FromResult<bool?>(null);
        }
    }
}
