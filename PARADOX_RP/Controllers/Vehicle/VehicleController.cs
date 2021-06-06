using AltV.Net.Async;
using AltV.Net.Data;
using Microsoft.EntityFrameworkCore;
using PARADOX_RP.Controllers.Inventory;
using PARADOX_RP.Controllers.Vehicle.Interface;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Game.Inventory;
using PARADOX_RP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
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

            if (vehicle.Inventory == null)
                vehicle.Inventory = await _inventoryController.CreateInventory(InventoryTypes.VEHICLE, dbVehicle.Id);

            await vehicle.SetNumberplateTextAsync(dbVehicle.Numberplate);

            await vehicle.SetPrimaryColorAsync((byte)dbVehicle.PrimaryColor);
            await vehicle.SetSecondaryColorAsync((byte)dbVehicle.SecondaryColor);

            vehicle.FuelType = dbVehicle.VehicleClass.FuelType;
            vehicle.Fuel = dbVehicle.Fuel;

            Pools.Instance.Register(dbVehicle.Id, vehicle);

            return await Task.FromResult(vehicle);
        }

        public async Task CreateDatabaseVehicle(int OwnerId, int VehicleClassId, int PrimaryColor = 0, int SecondaryColor = 0)
        {
            await using (var px = new PXContext())
            {
                Vehicles toInsert = new Vehicles()
                {
                    PlayerId = OwnerId,
                    VehicleClassId = VehicleClassId,
                    Numberplate = "PARADOX",
                    Parked = true,
                    CreatedAt = DateTime.Now,
                    GarageId = 1,

                    PrimaryColor = PrimaryColor,
                    SecondaryColor = SecondaryColor,
                };

                await px.Vehicles.AddAsync(toInsert);
                await px.SaveChangesAsync();
            }
        }

        public async Task<PXVehicle> CreateDatabaseVehicle(int OwnerId, int VehicleClassId, Position SpawnPosition, Rotation SpawnRotation, int PrimaryColor = 0, int SecondaryColor = 0)
        {
            await using (var px = new PXContext())
            {
                Vehicles toInsert = new Vehicles()
                {
                    PlayerId = OwnerId,
                    VehicleClassId = VehicleClassId,
                    Numberplate = "PARADOX",
                    Parked = false,
                    CreatedAt = DateTime.Now,
                    GarageId = 1,

                    Position_X = SpawnPosition.X,
                    Position_Y = SpawnPosition.Y,
                    Position_Z = SpawnPosition.Z,

                    Rotation_R = SpawnRotation.Roll,
                    Rotation_P = SpawnRotation.Pitch,
                    Rotation_Y = SpawnRotation.Yaw,

                    PrimaryColor = PrimaryColor,
                    SecondaryColor = SecondaryColor,
                };

                await px.Vehicles.AddAsync(toInsert);
                await px.SaveChangesAsync();

                var dbVehicle = await px.Vehicles.Include(v => v.VehicleClass).Where(v => v.Id == toInsert.Id).FirstOrDefaultAsync();
                return await CreateVehicle(dbVehicle);
            }
        }

        public async Task RemoveVehicle(PXVehicle vehicle)
        {
            _inventoryController.UnloadInventory(vehicle.Inventory.Id);
            Pools.Instance.Remove(vehicle.SqlId, vehicle);

            await vehicle.RemoveAsync();
        }
    }
}