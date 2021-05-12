using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Inventory;
using PARADOX_RP.Game.Inventory.Interfaces;
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
        public VehicleModule() : base("Vehicle")
        {

        }

        public Task<Inventories> OnInventoryOpen(PXPlayer player, Position position)
        {
            PXVehicle vehicle = Pools.Instance.Get<PXVehicle>(PoolType.VEHICLE).FirstOrDefault(v => v.Position.Distance(position) < 2.5);
            if (vehicle != null)
            {
                if (Configuration.Instance.DevMode)
                {
                    AltAsync.Log("Inventory found: " + vehicle.Inventory.Type + " " + vehicle.Inventory.Id);
                }

                return Task.FromResult(vehicle.Inventory);
            }

            return Task.FromResult<Inventories>(null);
        }

        public Task<bool?> CanAccessInventory(PXPlayer player, Inventories inventory)
        {
            if (inventory.Type != InventoryTypes.VEHICLE) return Task.FromResult<bool?>(null);

            PXVehicle vehicle = Pools.Instance.Get<PXVehicle>(PoolType.VEHICLE).FirstOrDefault(v => v.SqlId == inventory.TargetId);
            if (vehicle != null)
            {
                if (Configuration.Instance.DevMode)
                {
                    AltAsync.Log("Inventory can access: " + vehicle.Inventory.Type + " " + vehicle.Inventory.Id);
                }

                return Task.FromResult<bool?>(true);
            }

            return Task.FromResult<bool?>(null);
        }
    }
}
