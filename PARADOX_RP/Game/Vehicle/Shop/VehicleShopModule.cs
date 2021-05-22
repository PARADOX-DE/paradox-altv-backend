using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Vehicle.Shop
{
    class VehicleShopModule : ModuleBase<VehicleShopModule>
    {
        public VehicleShopModule() : base("VehicleShop")
        {

        }

        public override Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key)
        {
            if (key != KeyEnumeration.E) return Task.FromResult(false);

            

            return Task.FromResult(false);
        }
    }
}
