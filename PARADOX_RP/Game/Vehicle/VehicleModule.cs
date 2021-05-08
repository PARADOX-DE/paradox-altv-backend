using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Inventory;
using PARADOX_RP.Game.Inventory.Interfaces;
using PARADOX_RP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Vehicle
{
    class VehicleModule : ModuleBase<VehicleModule>, IInventoriable
    {
        public VehicleModule() : base("Vehicle")
        {

        }

        public Task<InventoryTypes> OnInventoryOpen(PXPlayer player, Position position)
        {
            IVehicle vehicle = Alt.GetAllVehicles().FirstOrDefault(v => v.Position.Distance(position) < 2.5);
            if (vehicle != null)
            {
                if (Configuration.Instance.DevMode)
                {
                    AltAsync.Log("Inventory found: VEHICLE");
                }

                return Task.FromResult(InventoryTypes.VEHICLE);
            }

            return Task.FromResult(InventoryTypes.PLAYER);
        }
    }
}
