using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using EntityStreamer;
using Microsoft.EntityFrameworkCore;
using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Controllers.Garage.Interface;
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
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Garage
{
    class GarageModule : ModuleBase<GarageModule>
    {
        private readonly IEventController _eventController;
        private readonly IVehicleController _vehicleController;
        private readonly IGarageController _garageController;
        private Dictionary<int, Garages> _garages = new Dictionary<int, Garages>();
        public GarageModule(PXContext pxContext, IEventController eventController, IVehicleController vehicleController, IGarageController garageController) : base("Garage")
        {
            _eventController = eventController;
            _vehicleController = vehicleController;
            _garageController = garageController;

            pxContext.Vehicles.ForEach((v) => v.Parked = true);
            pxContext.SaveChanges();

            LoadDatabaseTable(pxContext.Garages.Include(v => v.Vehicles).Include(v => v.Spawns), (Garages garage) =>
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
                MarkerStreamer.Create(MarkerTypes.MarkerTypeUpsideDownCone, g.Value.Position, new Vector3(1, 1, 1), new Vector3(0, 0, 0), null, new Rgba(37, 165, 202, 125));

                if (Configuration.Instance.DevMode)
                    g.Value.Spawns.ForEach((spawn) => MarkerStreamer.Create(MarkerTypes.MarkerTypeCarSymbol, spawn.Spawn_Position, new Vector3(1, 1, 1), new Vector3(0, 0, (float)(spawn.Spawn_Rotation.Yaw * 180 / Math.PI)), null, new Rgba(37, 165, 202, 200)));

            });
        }

        public override async Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key)
        {
            if (key != KeyEnumeration.E) return await Task.FromResult(false);
            if (!player.IsValid()) return await Task.FromResult(false);
            if (!player.CanInteract()) return await Task.FromResult(false);

            Garages dbGarage = _garages.Values.FirstOrDefault(g => g.Position.Distance(player.Position) < 3);
            if (dbGarage == null) return await Task.FromResult(false);

            WindowManager.Instance.Get<GarageWindow>().Show(player, await _garageController.RequestGarageVehicles(player, dbGarage));

            return await Task.FromResult(true);
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

                GarageSpawns garageSpawn = await _garageController.GetFreeGarageSpawn(dbGarage);
                if (garageSpawn == null)
                {
                    player.SendNotification("Garage", $"Derzeit sind leider alle Ausparkpunkte belegt.", NotificationTypes.ERROR);
                    return;
                }

                dbVehicle.Position_X = garageSpawn.Spawn_Position_X;
                dbVehicle.Position_Y = garageSpawn.Spawn_Position_Y;
                dbVehicle.Position_Z = garageSpawn.Spawn_Position_Z;

                dbVehicle.Rotation_R = garageSpawn.Spawn_Rotation_X;
                dbVehicle.Rotation_P = garageSpawn.Spawn_Rotation_Y;
                dbVehicle.Rotation_Y = garageSpawn.Spawn_Rotation_Z;

                dbVehicle.Parked = false;
                await px.SaveChangesAsync();

                await _vehicleController.CreateVehicle(dbVehicle);
                player.SendNotification("Garage", $"Fahrzeug {dbVehicle.VehicleModel.ToUpper()} wurde ausgeparkt.", NotificationTypes.ERROR);
            }
        }

        public async void GarageParkIn(PXPlayer player, int vehicleId, int garageId)
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

            PXVehicle vehicle = Pools.Instance.GetObjectById<PXVehicle>(PoolType.VEHICLE, vehicleId);
            if (vehicle == null)
            {
                /*
                 * VEHICLE ALREADY PARKED IN
                 */
                AltAsync.Log("Not found Object");
                return;
            }


            if (vehicle.Position.Distance(dbGarage.Position) > 30)
            {
                //TODO: ADD LOG
                return;
            }

            await using (var px = new PXContext())
            {
                Vehicles dbVehicle = await px.Vehicles.FindAsync(vehicleId);
                if (dbVehicle == null) return;


                if (dbVehicle.Parked)
                {
                    /*
                     * VEHICLE ALREADY PARKED
                     */
                    return;
                }

                dbVehicle.Parked = true;
                await px.SaveChangesAsync();

                Pools.Instance.Remove(vehicleId, vehicle);
                await vehicle.RemoveAsync();

                player.SendNotification("Garage", $"Fahrzeug {dbVehicle.VehicleModel.ToUpper()} wurde eingeparkt.", NotificationTypes.ERROR);
            }
        }
    }
}