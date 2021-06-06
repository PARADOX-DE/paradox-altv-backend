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
using PARADOX_RP.Core.Events;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Vehicle;
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

namespace PARADOX_RP.Game.GasStation
{
    class GasStationModule : ModuleBase<GasStationModule>, IEventKeyPressed, IEventPlayerConnect
    {
        private Dictionary<int, GasStations> _GasStations = new Dictionary<int, GasStations>();
        private Dictionary<int, GasStationPetrols> _GasStationPetrols = new Dictionary<int, GasStationPetrols>();

        private readonly IEventController _eventController;
        private readonly IVehicleController _vehicleController;

        public GasStationModule(PXContext px, IEventController eventController, IVehicleController vehicleController) : base("GasStation")
        {
            _eventController = eventController;
            _vehicleController = vehicleController;

            LoadDatabaseTable(px.GasStations, (GasStations gs) =>
            {
                _GasStations.Add(gs.Id, gs);
            });

            LoadDatabaseTable(px.GasStationPetrols, (GasStationPetrols gsp) =>
            {
                _GasStationPetrols.Add(gsp.Id, gsp);
            });

            _eventController.OnClient<PXPlayer, int, string, int>("PayGasStation", PayGasStation);
        }
        public void OnPlayerConnect(PXPlayer player)
        {
            _GasStations.ForEach((gs) =>
            {
                player.AddBlips($"Tankstelle", gs.Value.Position, 361, 81, 1, true);
            });
        }

        public async Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key)
        {
            if (key != KeyEnumeration.E) return await Task.FromResult(false);
            if (!player.IsValid()) return await Task.FromResult(false);
            if (!player.CanInteract()) return await Task.FromResult(false);

            GasStationPetrols dbPetrolStation = _GasStationPetrols.Values.FirstOrDefault(gsp => gsp.Position.Distance(player.Position) < 3);
            if (dbPetrolStation == null) return await Task.FromResult(false);

            if (!_GasStations.TryGetValue(dbPetrolStation.GasStationId, out GasStations dbGasStation)) return await Task.FromResult(false);

            PXVehicle nearestVehicle = Pools.Instance.Get<PXVehicle>(PoolType.VEHICLE).FirstOrDefault(v => v.Position.Distance(player.Position) < 5);
            if (nearestVehicle == null) return await Task.FromResult(true);

            VehicleClass vehicleClass = VehicleModule.Instance._vehicleClass.FirstOrDefault(v => v.Value.VehicleModel == nearestVehicle.VehicleModel).Value;
            if (vehicleClass == null) return await Task.FromResult(true);

            if (dbGasStation.OwnerId == -1) dbGasStation.TankVolume = 999999;

            if (dbGasStation.TankVolume <= 0)
            {
                player.SendNotification("Tankstelle", $"Die Tankstelle hat keinen Inhalt.", NotificationTypes.ERROR);
                return await Task.FromResult(true);
            }

            WindowController.Instance.Get<GasStationWindow>().Show(player, new GasStationWindowWriter(dbGasStation.Id, dbGasStation.Name, player.Money, dbGasStation.Petrol, dbGasStation.Diesel, dbGasStation.Electro, vehicleClass.MaxFuel-nearestVehicle.Fuel));

            return await Task.FromResult(true);
        }

        public async void PayGasStation(PXPlayer player, int GasStationId, string fuelType, int Volume)
        {
            if (!player.IsValid()) return;
            if (!player.CanInteract()) return;

            int price = -1;
            FuelTypes FuelType = FuelTypes.PETROL;

            if (!WindowController.Instance.Get<GasStationWindow>().IsVisible(player)) return;

            if (!_GasStations.TryGetValue(GasStationId, out GasStations dbGasStation)) return;

            PXVehicle nearestVehicle = Pools.Instance.Get<PXVehicle>(PoolType.VEHICLE).FirstOrDefault(v => v.Position.Distance(player.Position) < 5);
            if (nearestVehicle == null) return;

            VehicleClass vehicleClass = VehicleModule.Instance._vehicleClass.FirstOrDefault(v => v.Value.VehicleModel == nearestVehicle.VehicleModel).Value;
            if (vehicleClass == null) return;

            switch (fuelType)
            {
                case "Benzin":
                    price = dbGasStation.Petrol;
                    FuelType = FuelTypes.PETROL;
                    break;
                case "Diesel":
                    price = dbGasStation.Diesel;
                    FuelType = FuelTypes.DIESEL;
                    break;
                case "Elektrizität":
                    price = dbGasStation.Electro;
                    FuelType = FuelTypes.ELECTRO;
                    break;
            }

            if (price < 0) return;

            if (nearestVehicle.FuelType != FuelType)
            {
                // Falsch getankt
                if (Configuration.Instance.DevMode)
                    player.SendNotification("Tankstelle", $"Falsch Getankt.", NotificationTypes.ERROR);
                
                return;
            }

            if (dbGasStation.OwnerId == -1) dbGasStation.TankVolume = 999999;

            if (dbGasStation.TankVolume <= 0)
            {
                player.SendNotification("Tankstelle", $"Die Tankstelle hat keinen Inhalt.", NotificationTypes.ERROR);
                return;
            }

            if (Volume > dbGasStation.TankVolume)
            {
                Volume = dbGasStation.TankVolume;
                player.SendNotification("Tankstelle", $"Die Tankstelle hat nicht genug Inhalt um Vollständig zu tanken daher wird die Füllmenge auf {Volume} reduziert.", NotificationTypes.ERROR);
            }

            if((nearestVehicle.Fuel + Volume) > vehicleClass.MaxFuel)
            {
                if(FuelType == FuelTypes.ELECTRO)
                    player.SendNotification("Tankstelle", $"Soviel kWh passt nicht in deine Batterie!", NotificationTypes.ERROR);
                else
                    player.SendNotification("Tankstelle", $"Soviel {fuelType} passt nicht in deinen Tank!", NotificationTypes.ERROR);
             
                return;
            }

            if (!await player.TakeMoney(Volume * price))
            {
                player.SendNotification("Tankstelle", "Du hast nicht genug Geld dabei!", NotificationTypes.ERROR);
                return;
            }

            nearestVehicle.Fuel += Volume;
            if (FuelType == FuelTypes.ELECTRO)
                player.SendNotification("Tankstelle", $"Du hast dein Fahrzeug erfolgreich für {Volume * price}$ aufgeladen.", NotificationTypes.SUCCESS);
            else
                player.SendNotification("Tankstelle", $"Du hast dein Fahrzeug erfolgreich für {Volume * price}$ getankt.", NotificationTypes.SUCCESS);
            
        }
    }
}
