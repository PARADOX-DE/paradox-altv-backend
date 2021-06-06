using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Clothing.Models;
using PARADOX_RP.Utils;
using PARADOX_RP.Utils.Interface;
using System.Collections.Generic;
using System.Linq;

namespace PARADOX_RP.Game.Clothing
{
    class ClothesModule : ModuleBase<ClothesModule>
    {
        public List<ShopClothModel> _shopClothes = new List<ShopClothModel>();

        public ClothesModule(PXContext px, ILogger logger) : base("Clothes")
        {
            LoadDatabaseTable(px.Clothes, (Clothes c) =>
            {
                var mainCloth = _shopClothes.FirstOrDefault(s => s.Variants.FirstOrDefault(v => v.Value.Component == c.Component && v.Value.Drawable == c.Drawable).Value != null);
                if (mainCloth != null)
                {
                    mainCloth.Variants.Add(c.Id, c);
                }
                else
                {
                    _shopClothes.Add(new ShopClothModel()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Variants = new Dictionary<int, Clothes>() { { c.Id, c } },
                        Price = 0
                    });
                }
            });

            logger.Console(ConsoleLogType.SUCCESS, "Content", $"Successfully grouped {_shopClothes.Count}x Clothes!");
            //foreach(var c in _shopClothes)
            // {
            //    logger.Console(ConsoleLogType.SUCCESS, "LOG", $"{c.Name}");
            //} 
        }


    }
}
