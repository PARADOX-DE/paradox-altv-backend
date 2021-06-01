using AltV.Net.Async;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Clothing
{
    class ClothesModule : ModuleBase<ClothesModule>
    {
        public Dictionary<int, Clothes> _clothes = new Dictionary<int, Clothes>();
        
        public ClothesModule(PXContext px) : base("Clothes")
        {
            LoadDatabaseTable(px.Clothes, (Clothes c) =>
            {
                _clothes.Add(c.Id, c);
            });

            AltAsync.Log($"[+] Content >> Successfully loaded {_clothes.Count}x Clothes!");
        }

    }
}
