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

namespace PARADOX_RP.Game.GasStation
{
    class GasStationModule : ModuleBase<GasStationModule>
    {
        private Dictionary<int, GasStations> _GasStations = new Dictionary<int, GasStations>();
        private Dictionary<int, GasStationPetrols> _GasStationPetrols = new Dictionary<int, GasStationPetrols>();
        public GasStationModule(PXContext px, IEventController eventController, IVehicleController vehicleController) : base("GasStation")
        {
            LoadDatabaseTable(px.GasStations, (GasStations gs) =>
            {
                _GasStations.Add(gs.Id, gs);
            });

            LoadDatabaseTable(px.GasStationPetrols, (GasStationPetrols gsp) =>
            {
                _GasStationPetrols.Add(gsp.Id, gsp);
            });
        }
        public override void OnPlayerConnect(PXPlayer player)
        {
            _GasStations.ForEach((gs) =>
            {
                player.AddBlips($"Tankstelle", gs.Value.Position, 361, 81, 1, true);
            });
        }

        public override async Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key)
        {
            if (key != KeyEnumeration.E) return await Task.FromResult(false);
            if (!player.IsValid()) return await Task.FromResult(false);
            if (!player.CanInteract()) return await Task.FromResult(false);

            GasStationPetrols dbPetrolStation = _GasStationPetrols.Values.FirstOrDefault(gsp => gsp.Position.Distance(player.Position) < 3);
            if (dbPetrolStation == null) return await Task.FromResult(false);

            if (_GasStations.TryGetValue(dbPetrolStation.GasStationId, out GasStations dbGasStation))
            {
                WindowManager.Instance.Get<GasStationWindow>().Show(player, new GasStationWindowWriter(dbGasStation.Id, dbGasStation.Name, player.Money, dbGasStation.Petrol, dbGasStation.Diesel, dbGasStation.Electro));
            }

            return await Task.FromResult(true);
        }
    }
}
