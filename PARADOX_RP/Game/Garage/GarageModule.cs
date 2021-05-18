using AltV.Net;
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

            pxContext.Vehicles.ForEach((v) => v.Parked = true);
            pxContext.SaveChanges();

            LoadDatabaseTable(pxContext.Garages.Include(v => v.Vehicles), (Garages garage) =>
            {
                _garages.Add(garage.Id, garage);
            });

            _eventController.OnClient<PXPlayer, int, int>("GarageParkOut", GarageParkOut);
            _eventController.OnClient<PXPlayer, int, int>("GarageParkIn", GarageParkIn);
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

            Tuple<Vehicles, IEnumerable<Vehicles>> vehicles = await RequestGarageVehicles(player, dbGarage.Id);

            WindowManager.Instance.Get<GarageWindow>().Show(player, new GarageWindowWriter(dbGarage.Id, dbGarage.Name, vehicles));
            return await Task.FromResult(true);
        }

        // a) ParkIn Vehicle
        // b) Vehicles in Garage to park out 
        public async Task<Tuple<Vehicles, IEnumerable<Vehicles>>> RequestGarageVehicles(PXPlayer player, int garageId)
        {
            //very shitty needs rework
            if (!player.IsValid()) return null;
            if (!player.CanInteract()) return null;

            //if (!WindowManager.Instance.Get<GarageWindow>().IsVisible(player))
            //{
            /*
             * ADD LOGGER
             */
            //return null;
            //}

            if (!_garages.TryGetValue(garageId, out Garages dbGarage))
            {
                /*
                 * ADD LOGGER
                 */
                return null;
            }

            if (dbGarage.Position.Distance(player.Position) > 15)
            {
                /*
                * ADD LOGGER
                */
                return null;
            }


            await using (var px = new PXContext())
            {
                List<Vehicles> vehicles = await px.Vehicles.Where(v => v.GarageId == garageId && v.PlayerId == player.SqlId && v.Parked).ToListAsync();

                PXVehicle nearest = Pools.Instance.Get<PXVehicle>(PoolType.VEHICLE).FirstOrDefault(v => v.OwnerId == player.SqlId && (v.Position.Distance(dbGarage.Position) < 10));
                if (nearest == null) AltAsync.Log("vehicle not found");

                Vehicles dbNearest = await px.Vehicles.FindAsync(nearest?.SqlId);
                return new Tuple<Vehicles, IEnumerable<Vehicles>>(dbNearest, vehicles);
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
                if (dbVehicle == null) return;

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
                await px.SaveChangesAsync();

                await _vehicleController.CreateVehicle(dbVehicle);
                player.SendNotification("Garage", $"Fahrzeug {dbVehicle.VehicleModel.ToUpper()} wurde ausgeparkt.", NotificationTypes.ERROR);
                //TODO: change

            }
        }

        public async void GarageParkIn(PXPlayer player, int vehicleId, int garageId)
        {
            AltAsync.Log("GarageParkIn");

            if (!player.IsValid()) return;
            if (!player.CanInteract()) return;

            if (!WindowManager.Instance.Get<GarageWindow>().IsVisible(player))
            {
                /*
                 * ADD LOGGER
                 */
                AltAsync.Log("WindowManager");

                return;
            }

            if (!_garages.TryGetValue(garageId, out Garages dbGarage))
            {
                /*
                 * ADD LOGGER
                 */
                AltAsync.Log("Garages");

                return;
            }

            await using (var px = new PXContext())
            {
                Vehicles dbVehicle = await px.Vehicles.FindAsync(vehicleId);
                if (dbVehicle == null) return;

                if (!Pools.Instance.Find<PXVehicle>(PoolType.VEHICLE, dbVehicle.Id))
                {
                    /*
                     * VEHICLE NOT PARKED OUT
                     */
                    AltAsync.Log("VEHICLE NOT PARKED OUT");

                    return;
                }


                if (dbVehicle.Parked)
                {
                    /*
                     * VEHICLE ALREADY PARKED
                     */
                    AltAsync.Log("VEHICLE ALREADY PARKED");

                    return;
                }

                dbVehicle.Position_X = dbGarage.Spawn_Position_X;
                dbVehicle.Position_Y = dbGarage.Spawn_Position_Y;
                dbVehicle.Position_Z = dbGarage.Spawn_Position_Z;

                dbVehicle.Parked = true;

                PXVehicle vehicle = Pools.Instance.Get<PXVehicle>(PoolType.VEHICLE).FirstOrDefault(v => v.SqlId == dbVehicle.Id);
                if (vehicle == null)
                {
                    AltAsync.Log("vehicle");
                    return;
                }

                await px.SaveChangesAsync();

                Pools.Instance.Remove(vehicle.SqlId, vehicle);
                await vehicle.RemoveAsync();

                player.SendNotification("Garage", $"Fahrzeug {dbVehicle.VehicleModel.ToUpper()} wurde eingparkt.", NotificationTypes.ERROR);


                //TODO: change
            }
        }
    }
}