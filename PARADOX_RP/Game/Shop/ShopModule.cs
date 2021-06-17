using AltV.Net.Async.Elements.Refs;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Controllers.Inventory;
using PARADOX_RP.Controllers.Vehicle.Interface;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Events;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Shop.Models;
using PARADOX_RP.UI;
using PARADOX_RP.UI.Windows;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Shop
{
    class ShopModule : ModuleBase<ShopModule>, IEventKeyPressed, IEventPlayerConnect
    {
        private Dictionary<int, Shops> _shops = new Dictionary<int, Shops>();

        private readonly IEventController _eventController;
        private readonly IVehicleController _vehicleController;
        private readonly IInventoryController _inventoryController;

        public ShopModule(PXContext px, IEventController eventController, IVehicleController vehicleController, IInventoryController inventoryController) : base("Shop")
        {
            _eventController = eventController;
            _vehicleController = vehicleController;
            _inventoryController = inventoryController;

            LoadDatabaseTable(px.Shops.Include(s => s.Items), (Shops sp) =>
            {
                _shops.Add(sp.Id, sp);
            });

            _eventController.OnClient<PXPlayer, int, string>("PayShop", PayShop);
        }
        public void OnPlayerConnect(PXPlayer player)
        {
            _shops.ForEach((sp) => player.AddBlips(sp.Value.Name, sp.Value.Position, 52, 2, 1, true));
        }

        public async Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key)
        {
            if (key != KeyEnumeration.E) return await Task.FromResult(false);
            if (!player.IsValid()) return await Task.FromResult(false);
            if (!player.CanInteract()) return await Task.FromResult(false);

            Shops dbShop = _shops.Values.FirstOrDefault(sp => sp.Position.Distance(player.Position) < 3);
            if (dbShop == null) return await Task.FromResult(false);

            WindowController.Instance.Get<ShopWindow>().Show(player, new ShopWindowWriter(dbShop.Id, dbShop.Items));

            return await Task.FromResult(true);
        }

        private async void PayShop(PXPlayer player, int shopId, string cartString)
        {
            if (!player.IsValid()) return;
            if (!player.CanInteract()) return;

            Shops dbShop = _shops.Values.FirstOrDefault(sp => sp.Id == shopId && sp.Position.Distance(player.Position) < 10);
            if (dbShop == null)
            {
                // LOGGER
                return;
            }

            List<ShopCartModel> shopCart = JsonConvert.DeserializeObject<List<ShopCartModel>>(cartString);
            if (shopCart == null) return;

            int cartPrice = 0;
            shopCart.ForEach((shopItem) =>
            {
                ShopItems dbShopItem = dbShop.Items.FirstOrDefault((i) => i.Id == shopItem.id);
                if (dbShopItem == null) return;

                cartPrice += (dbShopItem.Price * shopItem.amount);
            });

            if (await player.TakeMoney(cartPrice))
            {
                shopCart.ForEach((shopItem) =>
                {
                    ShopItems dbShopItem = dbShop.Items.FirstOrDefault((i) => i.Id == shopItem.id);
                    if (dbShopItem == null) return;

                    _inventoryController.CreateItem(player.Inventory, dbShopItem.ItemId, shopItem.amount, $"Shopkauf von {player.Username}");
                });
                player.SendNotification(ModuleName, "Du hast die Gegenstände in deinem Warenkorb erfolgreich gekauft.", NotificationTypes.SUCCESS);
            }
            else
                player.SendNotification(ModuleName, $"Dir fehlen {cartPrice - player.Money}$ um dir das zu kaufen.", NotificationTypes.ERROR);
            
        }

        public Shops GetShopById(int Id)
        {
            if (_shops.TryGetValue(Id, out Shops shop)) return shop;
            return null;
        }
    }
}
