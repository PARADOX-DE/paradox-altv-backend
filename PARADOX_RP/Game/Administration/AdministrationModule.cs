using PARADOX_RP.Core.Module;
using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PARADOX_RP.Utils.Enums;

namespace PARADOX_RP.Game.Administration
{
    class AdministrationModule : ModuleBase<AdministrationModule>
    {
        public AdministrationModule() : base("Administration") { }

        public override Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key)
        {
            if(key == KeyEnumeration.F9)
            {
                //go aduty
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
