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
using PARADOX_RP.Game.Vehicle;
using System.Linq;
using PARADOX_RP.Controllers.Inventory;
using PARADOX_RP.Game.Inventory;
using PARADOX_RP.Core.Events;
using PARADOX_RP.Controllers.Weapon.Interface;
using PARADOX_RP.Game.Commands.Extensions;

namespace PARADOX_RP.Game.Administration
{
    class AdministrationModule : ModuleBase<AdministrationModule>, ICommand, IEventKeyPressed
    {
        private IVehicleController _vehicleController;
        private IInventoryController _inventoryController;
        private IWeaponController _weaponController;
        public AdministrationModule(IVehicleController vehicleController, IInventoryController inventoryController, IWeaponController weaponController) : base("Administration")
        {
            _vehicleController = vehicleController;
            _inventoryController = inventoryController;
            _weaponController = weaponController;
        }

        private readonly List<ClothesVariants> _administrativeOutfit = new List<ClothesVariants>()
        {
            new ClothesVariants() { Component = 1, Drawable = 134, Texture = 3 },
            new ClothesVariants() { Component = 11, Drawable = 274, Texture = 3 },
            new ClothesVariants() { Component = 4, Drawable = 106, Texture = 3 },
            new ClothesVariants() { Component = 8, Drawable = 15, Texture = 3 },
            new ClothesVariants() { Component = 3, Drawable = 9, Texture = 3 },
            new ClothesVariants() { Component = 6, Drawable = 83, Texture = 3 }
        };

        public async Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key)
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

                WindowController.Instance.Get<NativeMenuWindow>().DisplayMenu<AdministrationNativeMenu>(player);
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
                await player.SetPedIntoVeh(vehicle, -1);
            }
            catch { player.SendNotification(ModuleName, $"Fahrzeug nicht gefunden.", NotificationTypes.ERROR); }
        }

        [Command("sendconfirmation", true)]
        public void CommandSendConfirmation(PXPlayer player, string Title, string Description)
        {
            if (!player.IsValid()) return;
            if (!PermissionsModule.Instance.HasPermissions(player)) return;

            WindowController.Instance.Get<ConfirmationWindow>().Show(player, new ConfirmationWindowWriter(Title, Description, "", ""));
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
            try
            {
                if (!PermissionsModule.Instance.HasPermissions(player)) return;

                await _weaponController.AddWeapon(player, weaponModel);
            }
            catch { player.SendNotification(ModuleName, "Waffe nicht gefunden.", NotificationTypes.SUCCESS); }
        }

        [Command("pos")]
        public void CommandPos(PXPlayer player, string positionName)
        {
            if (!PermissionsModule.Instance.HasPermissions(player)) return;

            Position position;
            Rotation rotation;

            if (player.Vehicle != null)
            {
                lock (player.Vehicle)
                {
                    position = player.Vehicle.Position;
                    rotation = player.Vehicle.Rotation;
                }
            }
            else
            {
                position = player.Position;
                rotation = player.Rotation;
            }

            player.SendNotification("Position", "Position an die Konsole gesendet.", NotificationTypes.SUCCESS);
            Alt.Log($"{positionName} X: {position.X.ToString().Replace(",", ".")} Y: {position.Y.ToString().Replace(",", ".")} Z: {position.Z.ToString().Replace(",", ".")} | ROT: {rotation.Yaw.ToString().Replace(",", ".")}");
        }

        [Command("createveh")]
        public async void CommandCreateDatabaseVeh(PXPlayer player, string VehicleModel)
        {
            VehicleClass vehicleClass = VehicleModule.Instance._vehicleClass.FirstOrDefault(v => v.Value.VehicleModel.ToLower().StartsWith(VehicleModel.ToLower())).Value;
            if (vehicleClass == null)
            {
                player.SendNotification("Administration", $"Fahrzeugmodell {VehicleModel} nicht gefunden.", NotificationTypes.SUCCESS);
                return;
            }

            PXVehicle result = await _vehicleController.CreateDatabaseVehicle(player.SqlId, vehicleClass.Id, player.Position, player.Rotation);
            await player.SetPedIntoVeh(result, -1);
        }

        [Command("additem")]
        public async void CommandAddItem(PXPlayer player, int ItemId, int Amount)
        {
            if (!InventoryModule.Instance._items.TryGetValue(ItemId, out Items Item))
            {
                player.SendChatMessage("AddItem", $"Item {ItemId} konnte nicht gefunden werden.", true);
                return;
            }

            await _inventoryController.CreateItem(player.Inventory, ItemId, Amount, "Administrativ erstellt von " + player.Username);
            player.SendChatMessage("Command", $"Du hast dir 1x {Item.Name} gegeben.");
        }

        [Command("global")]
        public void CommandGlobal(PXPlayer player, string Title, string Message, int Duration)
        {
            player.SendChatMessage("Global", Title + " " + Message);
        }
    }
}
