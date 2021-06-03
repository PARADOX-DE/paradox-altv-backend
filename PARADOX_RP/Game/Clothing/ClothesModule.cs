using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Utils;
using PARADOX_RP.Utils.Interface;
using System.Collections.Generic;

namespace PARADOX_RP.Game.Clothing
{
    class ClothesModule : ModuleBase<ClothesModule>
    {
        public Dictionary<int, Clothes> _clothes = new Dictionary<int, Clothes>();
        
        public ClothesModule(PXContext px, ILogger logger) : base("Clothes")
        {
            LoadDatabaseTable(px.Clothes, (Clothes c) =>
            {
                _clothes.Add(c.Id, c);
            });

            logger.Console(ConsoleLogType.SUCCESS, "Content", $"Successfully loaded {_clothes.Count}x Clothes!");
        }

        
    }
}
