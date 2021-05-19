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

            LoadDatabaseTable(pxContext.Garages.Include(v => v.Vehicles).Include(v => v.Spawns), (Garages garage) =>
            {
                _garages.Add(garage.Id, garage);
            });

            //_eventController.OnClient<PXPlayer, int, int>("GarageParkOut", GarageParkOut);
            //_eventController.OnClient<PXPlayer, int, int>("GarageParkIn", GarageParkIn);
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

            return await Task.FromResult(true);
        }

        private bool CanParkOutVehicle(PXPlayer player, Vehicles dbVehicle, Garages garage) => dbVehicle.GarageId == garage.Id && dbVehicle.PlayerId == player.SqlId && dbVehicle.Parked;

        public async Task<GarageWindowWriter> RequestGarageVehicles(PXPlayer player, Garages garage)
        {
            PXVehicle tmpNearestVehicle = Pools.Instance.Get<PXVehicle>(PoolType.VEHICLE).FirstOrDefault(v => v.OwnerId == player.SqlId && (v.Position.Distance(garage.Position) < 20));
            GarageWindowVehicle NearestVehicle = tmpNearestVehicle == null ? null : new GarageWindowVehicle(tmpNearestVehicle.Id, tmpNearestVehicle.VehicleModel);

            await using (var px = new PXContext())
            {
                List<GarageWindowVehicle> Vehicles = new List<GarageWindowVehicle>();
                await px.Vehicles.Where(v => CanParkOutVehicle(player, v, garage)).ForEachAsync((v) =>
                {
                    Vehicles.Add(new GarageWindowVehicle(v.Id, v.VehicleModel));
                });

                return new GarageWindowWriter(garage.Id, garage.Name, Vehicles, NearestVehicle);
            }
        }
    }
}