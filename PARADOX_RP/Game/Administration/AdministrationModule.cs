using PARADOX_RP.Core.Module;
using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PARADOX_RP.Utils.Enums;
using AltV.Net.Async;
using AltV.Net;
using PARADOX_RP.Game.Commands;
using PARADOX_RP.Game.Commands.Attributes;
using PARADOX_RP.Controllers.Vehicle.Interface;

namespace PARADOX_RP.Game.Administration
{
    class AdministrationModule : ModuleBase<AdministrationModule>, ICommand
    {
        private IVehicleController _vehicleController;
        public AdministrationModule(IVehicleController vehicleController) : base("Administration") {
            _vehicleController = vehicleController;
        }

        public override async Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key)
        {
            if (key == KeyEnumeration.F9)
            {
                if (player.DutyType != DutyTypes.ADMINDUTY) await EnterAduty(player);
                else await LeaveAduty(player);

                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        public async Task EnterAduty(PXPlayer player)
        {
            if (PermissionsModule.Instance.HasPermissions(player))
            {
                player.DutyType = DutyTypes.ADMINDUTY;
                await player.EmitAsync("UpdateAdminDuty");
            }
        }

        public async Task LeaveAduty(PXPlayer player)
        {
            if (PermissionsModule.Instance.HasPermissions(player))
            {
                if (player.DutyType != DutyTypes.ADMINDUTY) return;

                player.DutyType = DutyTypes.OFFDUTY;
                await player.EmitAsync("UpdateAdminDuty");
            }
        }


        [Command("aduty")]
        public async void aduty(PXPlayer player)
        {
            await Instance.OnKeyPress(player, KeyEnumeration.F9);
        }

        [Command("veh")]
        public async void veh(PXPlayer player, string vehicleModel)
        {
            try
            {
                PXVehicle vehicle = (PXVehicle)await AltAsync.CreateVehicle(vehicleModel, player.Position, player.Rotation);
            }
            catch { player.SendNotification(ModuleName, $"Fahrzeug nicht gefunden.", NotificationTypes.ERROR); }
        }
    }
}
