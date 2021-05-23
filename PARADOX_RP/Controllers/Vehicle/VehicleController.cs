using AltV.Net.Async;
using AltV.Net.Data;
using PARADOX_RP.Controllers.Inventory;
using PARADOX_RP.Controllers.Vehicle.Interface;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Game.Inventory;
using PARADOX_RP.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Controllers.Vehicle
{
    class VehicleController : IVehicleController
    {
        private IInventoryController _inventoryController;

        public VehicleController(IInventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }

        public async Task<PXVehicle> CreateVehicle(Vehicles dbVehicle)
        {
            PXVehicle vehicle = (PXVehicle)await AltAsync.CreateVehicle(dbVehicle.VehicleClass.VehicleModel, dbVehicle.Position, dbVehicle.Rotation);
            vehicle.SqlId = dbVehicle.Id;
            vehicle.VehicleModel = dbVehicle.VehicleClass.VehicleModel;
            vehicle.OwnerId = dbVehicle.PlayerId;
            vehicle.Inventory = await _inventoryController.LoadInventory(InventoryTypes.VEHICLE, dbVehicle.Id);

            Pools.Instance.Register(dbVehicle.Id, vehicle);

            return await Task.FromResult(vehicle);
        }
    }
}
