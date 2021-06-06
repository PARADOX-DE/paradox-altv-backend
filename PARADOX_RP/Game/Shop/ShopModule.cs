using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Controllers.Vehicle.Interface;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Events;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
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
        private Dictionary<int, Shops> _Shops = new Dictionary<int, Shops>();
        private Dictionary<int, ShopItems> _ShopItems = new Dictionary<int, ShopItems>();

        private readonly IEventController _eventController;
        private readonly IVehicleController _vehicleController;

        public ShopModule(PXContext px, IEventController eventController, IVehicleController vehicleController) : base("GasStation")
        {
            _eventController = eventController;
            _vehicleController = vehicleController;;

            LoadDatabaseTable(px.Shops, (Shops sp) =>
            {
                _Shops.Add(sp.Id, sp);
            });

            LoadDatabaseTable(px.ShopItems, (ShopItems item) =>
            {
                _ShopItems.Add(item.Id, item);

                if (!_Shops.TryGetValue(item.ShopsId, out Shops shop)) return;

                shop.Items.Add(new ShopItem(item.Id, item.Name, item.Price));
            });

            //_eventController.OnClient<PXPlayer, int, string, int>("PayShop", PayShop);
        }

        public void OnPlayerConnect(PXPlayer player)
        {
            _Shops.ForEach((sp) =>
            {
                player.AddBlips($"Shop", sp.Value.Position, 52, 2, 1, true);
            });
        }

        public async Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key)
        {
            if (key != KeyEnumeration.E) return await Task.FromResult(false);
            if (!player.IsValid()) return await Task.FromResult(false);
            if (!player.CanInteract()) return await Task.FromResult(false);

            Shops dbShop = _Shops.Values.FirstOrDefault(sp => sp.Position.Distance(player.Position) < 3);
            if (dbShop == null) return await Task.FromResult(false);

            WindowController.Instance.Get<ShopWindow>().Show(player, new ShopWindowWriter(dbShop.Id, dbShop.Items));

            return await Task.FromResult(true);
        }
    }
}
