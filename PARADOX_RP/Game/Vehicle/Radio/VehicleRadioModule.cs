using AltV.Net.Async;
using AltV.Net.Elements.Entities;
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
        public VehicleRadioModule() : base("VehicleRadio") {
            AltAsync.OnClient<PXPlayer>("EnableVehicleRadio", EnableVehicleRadio);
            AltAsync.OnClient<PXPlayer>("DisableVehicleRadio", DisableVehicleRadio);
        }

        private async void DisableVehicleRadio(PXPlayer obj)
        {
            throw new NotImplementedException();
        }

        private async void EnableVehicleRadio(PXPlayer player)
        {

        }

        public override async Task OnPlayerEnterVehicle(IVehicle v, IPlayer p, byte seat)
        {
            PXVehicle vehicle = (PXVehicle)v;
            if (!vehicle.IsValid()) return;

            if (vehicle.HasRadio)
            {
                await p.EmitAsync("EnableVehicleRadio", Configuration.Instance.VehicleRadioURL);
            }
        }

        public override async Task OnPlayerLeaveVehicle(IVehicle v, IPlayer p, byte seat)
        {
            PXVehicle vehicle = (PXVehicle)v;
            if (!vehicle.IsValid()) return;

            if (vehicle.HasRadio)
            {
                await p.EmitAsync("DisableVehicleRadio");
            }
        }
    }
}
