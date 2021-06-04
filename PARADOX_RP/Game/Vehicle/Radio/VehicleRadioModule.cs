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
    class VehicleRadioModule : ModuleBase<VehicleRadioModule>, IEventPlayerVehicle
    {
        public VehicleRadioModule() : base("VehicleRadio")
        {
            AltAsync.OnClient<PXPlayer>("EnableVehicleRadio", EnableVehicleRadio);
            AltAsync.OnClient<PXPlayer>("DisableVehicleRadio", DisableVehicleRadio);
        }

        private async void DisableVehicleRadio(PXPlayer player)
        {
            if (player.Seat != Convert.ToByte(VehicleSeats.DRIVER) && player.Seat != Convert.ToByte(VehicleSeats.CODRIVER)) return;

            PXVehicle vehicle = (PXVehicle)player.Vehicle;
            if (!vehicle.IsValid()) return;

            if (vehicle.HasRadio)
            {
                await player.EmitAsync("DisableVehicleRadio");
            }
        }

        private async void EnableVehicleRadio(PXPlayer player)
        {
            if (player.Seat != Convert.ToByte(VehicleSeats.DRIVER) && player.Seat != Convert.ToByte(VehicleSeats.CODRIVER)) return;

            PXVehicle vehicle = (PXVehicle)player.Vehicle;
            if (!vehicle.IsValid()) return;

            if (vehicle.HasRadio)
            {
                await player.EmitAsync("EnableVehicleRadio", Configuration.Instance.VehicleRadioURL);
            }
        }

        public async Task OnPlayerEnterVehicle(IVehicle v, IPlayer p, byte seat)
        {
            PXVehicle vehicle = (PXVehicle)v;
            if (!vehicle.IsValid()) return;

            if (vehicle.HasRadio)
            {
                await p.EmitAsync("EnableVehicleRadio", Configuration.Instance.VehicleRadioURL);
            }
        }

        public async Task OnPlayerLeaveVehicle(IVehicle v, IPlayer p, byte seat)
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
