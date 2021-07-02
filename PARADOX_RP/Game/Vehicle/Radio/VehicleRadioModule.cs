using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using PARADOX_RP.Core.Events;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Utils;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Vehicle.Radio
{
    class VehicleRadioModule : ModuleBase<VehicleRadioModule>
    {
        public VehicleRadioModule() : base("VehicleRadio")
        {
            AltAsync.OnClient<PXPlayer>("ToggleVehicleRadio", ToggleVehicleRadio);
        }

        private async void ToggleVehicleRadio(PXPlayer player)
        {
            PXVehicle vehicle = (PXVehicle)player.Vehicle;
            if (!vehicle.IsValid() || vehicle.Driver.Id != player.Id) return;

            await vehicle.SetMetaDataAsync("RadioChangedDate", DateTime.Now);
            await vehicle.SetStreamSyncedMetaDataAsync("HasRadio", !vehicle.HasRadio);
            vehicle.HasRadio = !vehicle.HasRadio;
        }
    }
}
