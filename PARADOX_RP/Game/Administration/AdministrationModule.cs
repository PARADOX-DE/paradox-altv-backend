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
using PARADOX_RP.Game.Clothing.Extensions;
using PARADOX_RP.Core.Database.Models;
using AltV.Net.Data;
using PARADOX_RP.UI;
using PARADOX_RP.UI.Windows;
using Newtonsoft.Json;
using PARADOX_RP.UI.Windows.NativeMenu;
using PARADOX_RP.Game.Administration.NativeMenu;
using AltV.Net.Enums;

namespace PARADOX_RP.Game.Administration
{
    class AdministrationModule : ModuleBase<AdministrationModule>, ICommand
    {
        private IVehicleController _vehicleController;
        public AdministrationModule(IVehicleController vehicleController) : base("Administration")
        {
            _vehicleController = vehicleController;
        }

        private readonly List<Clothes> _administrativeOutfit = new List<Clothes>()
        {
            new Clothes() { Component = 1, Drawable = 134, Texture = 3 },
            new Clothes() { Component = 11, Drawable = 274, Texture = 3 },
            new Clothes() { Component = 4, Drawable = 106, Texture = 3 },
            new Clothes() { Component = 8, Drawable = 15, Texture = 3 },
            new Clothes() { Component = 3, Drawable = 9, Texture = 3 },
            new Clothes() { Component = 6, Drawable = 83, Texture = 3 }
        };

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

                _administrativeOutfit.ForEach(async (c) =>
                {
                    await player.SetClothes(c.Component, c.Drawable, c.Texture);
                });

                WindowManager.Instance.Get<NativeMenuWindow>().DisplayMenu<AdministrationNativeMenu>(player);
            }
        }

        public async Task LeaveAduty(PXPlayer player)
        {
            if (PermissionsModule.Instance.HasPermissions(player))
            {
                if (player.DutyType != DutyTypes.ADMINDUTY) return;

                player.DutyType = DutyTypes.OFFDUTY;
                await player.EmitAsync("UpdateAdminDuty");

                player.AssignLoadedClothes();
            }
        }


        [Command("aduty")]
        public async void aduty(PXPlayer player)
        {
            await Instance.OnKeyPress(player, KeyEnumeration.F9);
        }

        [Command("veh")]
        public async void CommandVeh(PXPlayer player, string vehicleModel)
        {
            if (!PermissionsModule.Instance.HasPermissions(player)) return;

            try
            {
                PXVehicle vehicle = (PXVehicle)await AltAsync.CreateVehicle(vehicleModel, player.Position, player.Rotation);
            }
            catch { player.SendNotification(ModuleName, $"Fahrzeug nicht gefunden.", NotificationTypes.ERROR); }
        }

        [Command("sendconfirmation", true)]
        public void CommandSendConfirmation(PXPlayer player, string Title, string Description)
        {
            if (!player.IsValid()) return;
            if (!PermissionsModule.Instance.HasPermissions(player)) return;

            WindowManager.Instance.Get<ConfirmationWindow>().Show(player, new ConfirmationWindowWriter(Title, Description, "", ""));
        }

        [Command("clothes")]
        public async void CommandClothes(PXPlayer player, int component, int drawable, int texture)
        {
            if (!PermissionsModule.Instance.HasPermissions(player)) return;

            await player.SetClothes(component, drawable, texture);
        }

        [Command("weapon")]
        public async void CommandWeapon(PXPlayer player, WeaponModel weaponModel)
        {
            if (!PermissionsModule.Instance.HasPermissions(player)) return;

            await player.GiveWeaponAsync((uint)weaponModel, 9999, true);
        }

        [Command("pos")]
        public void CommandPos(PXPlayer player, string positionName)
        {
            if (!PermissionsModule.Instance.HasPermissions(player)) return;

            Position position;
            Rotation rotation;

            if (player.Vehicle != null)
            {
                position = player.Vehicle.Position;
                rotation = player.Vehicle.Rotation;
            }
            else
            {
                position = player.Position;
                rotation = player.Rotation;
            }

            player.SendNotification("Position", "Position an die Konsole gesendet.", NotificationTypes.SUCCESS);
            Alt.Log($"{positionName} X: {position.X.ToString().Replace(",", ".")} Y: {position.Y.ToString().Replace(",", ".")} Z: {position.Z.ToString().Replace(",", ".")} | ROT: {rotation.Yaw.ToString().Replace(",", ".")}");
        }
    }
}
