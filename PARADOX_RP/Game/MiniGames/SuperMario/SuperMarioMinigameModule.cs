using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.MiniGames.SuperMario
{
    class SuperMarioMinigameModule : ModuleBase<SuperMarioMinigameModule>
    {
        public SuperMarioMinigameModule() : base("SuperMarioMinigame") { }

        public override async Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key)
        {
            if(key == KeyEnumeration.E)
            {

            }

            return await Task.FromResult(false);
        }
    }
}
