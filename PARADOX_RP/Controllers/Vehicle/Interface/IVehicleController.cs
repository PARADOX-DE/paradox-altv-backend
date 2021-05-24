using AltV.Net.Data;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Controllers.Vehicle.Interface
{
    interface IVehicleController
    {
        Task<PXVehicle> CreateVehicle(Vehicles dbVehicle);
        Task CreateDatabaseVehicle(int OwnerId, int VehicleClassId);
        Task<PXVehicle> CreateDatabaseVehicle(int OwnerId, int VehicleClassId, Position SpawnPosition, Rotation SpawnRotation);
    }
}
