using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Handlers.Inventory;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Inventory
{
    public enum InventoryTypes
    {
        PLAYER,
        VEHICLE,
        TEAMHOUSE,
        STORAGEROOM,
        LOCKER
    }

    class InventoryModule : ModuleBase<InventoryModule>
    {
        public readonly IInventoryHandler _inventoryHandler;
        public InventoryModule(IInventoryHandler inventoryHandler) : base("Inventory") {
            _inventoryHandler = inventoryHandler;
        }

        public async void OpenInventory()
        {
            await _inventoryHandler.LoadInventory(InventoryTypes.VEHICLE, 1);
        }
    }
}
