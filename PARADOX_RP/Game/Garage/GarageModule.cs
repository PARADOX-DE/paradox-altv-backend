using AltV.Net.Async;
using Microsoft.EntityFrameworkCore;
using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Controllers.Vehicle.Interface;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.UI;
using PARADOX_RP.UI.Windows;
using PARADOX_RP.Utils;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Garage
{
    class GarageModule : ModuleBase<GarageModule>
    {
        private readonly IEventController _eventController;
        private readonly IVehicleController _vehicleController;
        private Dictionary<int, Garages> _garages = new Dictionary<int, Garages>();
        public GarageModule(PXContext pxContext, IEventController eventController, IVehicleController vehicleController) : base("Garage")
        {
            _eventController = eventController;
            _vehicleController = vehicleController;

            LoadDatabaseTable(pxContext.Garages.Include(v => v.Vehicles), (Garages garage) =>
            {
                _garages.Add(garage.Id, garage);
            });

            _eventController.OnClient<PXPlayer, int, int>("GarageParkOut", GarageParkOut);
        }

        public override void OnPlayerConnect(PXPlayer player)
        {
            _garages.ForEach((g) =>
            {
                player.AddBlips(g.Value.Name, g.Value.Position, 524, 0, 1, true);
            });
        }

        public override async Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key)
        {
            if (key != KeyEnumeration.E) return await Task.FromResult(false);
            if (!player.IsValid()) return await Task.FromResult(false);
            if (!player.CanInteract()) return await Task.FromResult(false);

            Garages dbGarage = _garages.Values.FirstOrDefault(g => g.Position.Distance(player.Position) < 3);
            if (dbGarage == null) return await Task.FromResult(false);

            dbGarage.Vehicles.
            WindowManager.Instance.Get<GarageWindow>().Show(player, new GarageWindowWriter(dbGarage.Id, dbGarage.Name, dbGarage.Vehicles.Where(p => p.Parked == true)));
            return await Task.FromResult(true);
        }

        public async void RequestGarageVehicles(PXPlayer player, int garageId)
        {
            if (!player.IsValid()) return;
            if (!player.CanInteract()) return;

            if (!WindowManager.Instance.Get<GarageWindow>().IsVisible(player))
            {
                /*
                 * ADD LOGGER
                 */
                return;
            }

            if (!_garages.TryGetValue(garageId, out Garages dbGarage))
            {
                /*
                 * ADD LOGGER
                 */
                return;
            }

            if (dbGarage.Position.Distance(player.Position) > 10)
            {
                /*
                * ADD LOGGER
                */
                return;
            }


            await using (var px = new PXContext())
            {
                IEnumerable<Vehicles> vehicles = px.Vehicles.Where(v => v.GarageId == garageId);


            }
        }

        public async void GarageParkOut(PXPlayer player, int vehicleId, int garageId)
        {
            if (!player.IsValid()) return;
            if (!player.CanInteract()) return;

            if (!WindowManager.Instance.Get<GarageWindow>().IsVisible(player))
            {
                /*
                 * ADD LOGGER
                 */
                return;
            }

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

                if (Pools.Instance.Find<PXVehicle>(PoolType.VEHICLE, dbVehicle.Id))
                {
                    /*
                     * VEHICLE ALREADY PARKED OUT
                     */
                    return;
                }


                if (!dbVehicle.Parked)
                {
                    /*
                     * VEHICLE ALREADY PARKED OUT
                     */
                    return;
                }

                dbVehicle.Position_X = dbGarage.Spawn_Position_X;
                dbVehicle.Position_Y = dbGarage.Spawn_Position_Y;
                dbVehicle.Position_Z = dbGarage.Spawn_Position_Z;

                dbVehicle.Parked = false;
                _garages[garageId].Vehicles.FirstOrDefault(i => i.Id == dbVehicle.Id).Parked = false;

                await _vehicleController.CreateVehicle(dbVehicle);
                await px.SaveChangesAsync();
                //TODO: change
            }
        }

    }
}
