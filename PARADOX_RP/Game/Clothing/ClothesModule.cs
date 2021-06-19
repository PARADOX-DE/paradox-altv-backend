using AltV.Net.Data;
using AltV.Net.Async;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Events;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Clothing.Models;
using PARADOX_RP.Utils;
using PARADOX_RP.Utils.Enums;
using PARADOX_RP.Utils.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PARADOX_RP.UI;
using PARADOX_RP.Controllers.UI.Windows.ClothShop;

namespace PARADOX_RP.Game.Clothing
{
    class ClothesModule : ModuleBase<ClothesModule>, IEventKeyPressed
    {
        public Dictionary<int, ClothesShop> _clothesShops = new Dictionary<int, ClothesShop>();

        // need to split to database table, just for debug reasons
        public List<ShopClothModel> _shopClothes = new List<ShopClothModel>();

        public ClothesModule(PXContext px, ILogger logger) : base("Clothes")
        {
            int _autoIncrement = 0;
            LoadDatabaseTable(px.Clothes, (Clothes c) =>
            {
                var mainCloth = _shopClothes.Find(s => (int)s.Gender == c.Gender && s.Variants.Any(i => i.Value.Component == c.Component && i.Value.Drawable == c.Drawable));
                if (mainCloth != null)
                {
                    mainCloth.Variants.Add(c.Id, c);
                }
                else
                {
                    _autoIncrement++;

                    _shopClothes.Add(new ShopClothModel()
                    {
                        Id = _autoIncrement,
                        Name = c.Name,
                        ComponentVariation = (ComponentVariation)c.Component,
                        Variants = new Dictionary<int, Clothes>() { { c.Id, c } },
                        Price = 0,
                        Gender = (Gender)c.Gender
                    });
                }
            });

            logger.Console(ConsoleLogType.SUCCESS, "Content", $"Successfully grouped {_shopClothes.Count}x Clothes!");
        }

        public Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key)
        {
            if (key != KeyEnumeration.E) return Task.FromResult(false);
            if (!player.IsValid()) return Task.FromResult(false);
            if (!player.CanInteract()) return Task.FromResult(false);

            var playerPos = Position.Zero; player.GetPositionLocked(ref playerPos);
            ClothesShop clothesShop = _clothesShops.Values.FirstOrDefault(g => g.Position.Distance(playerPos) <= 5);
            if (clothesShop == null) return Task.FromResult(false);

            WindowController.Instance.Get<ClothShopWindow>().Show(player, new ClothShopWindowWriter(_shopClothes.Where(s => s.Gender == 0).ToList()));

            return Task.FromResult(true);
        }
    }
}
