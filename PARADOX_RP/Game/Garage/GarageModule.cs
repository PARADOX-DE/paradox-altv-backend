using PARADOX_RP.Controllers.Vehicle.Interface;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Garage
{
    class GarageModule : ModuleBase<GarageModule>
    {
        private readonly IVehicleController _vehicleController;
        private Dictionary<int, Garages> _garages;
        public GarageModule(IVehicleController vehicleController) : base("Garage")
        {
            _vehicleController = vehicleController;
            _garages = new Dictionary<int, Garages>();
        }

        public async void GarageParkOut(PXPlayer player, int vehicleId, int garageId)
        {
            if (!_garages.TryGetValue(garageId, out Garages dbGarage))
            {
                /*
                 * ADD LOGGER
                 */
                return;
            }

            await using (var px = new PXContext())
            {
                Vehicles dbVehicle = await px.Vehicles.FindAsync(vehicleId);
                if (dbVehicle.GarageId != garageId)
                {
                    /*
                    * ADD LOGGER
                    */
                    return;
                }

                if(Pools.Instance.Find<PXVehicle>(PoolType.VEHICLE, dbVehicle.Id))
                {
                    /*
                     * VEHICLE ALREADY PARKED OUT
                     */
                    return;
                }

                dbVehicle.Position_X = dbGarage.Spawn_Position_X;
                dbVehicle.Position_Y = dbGarage.Spawn_Position_Y;
                dbVehicle.Position_Z = dbGarage.Spawn_Position_Z;
                await _vehicleController.CreateVehicle(dbVehicle);
                await px.SaveChangesAsync();
                //TODO: change
            }
        }

    }
}
