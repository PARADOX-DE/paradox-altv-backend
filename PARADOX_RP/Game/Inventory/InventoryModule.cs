﻿using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Controllers.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Game.Inventory.Interfaces;

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
        public readonly IInventoryController _inventoryHandler;
        public InventoryModule(IInventoryController inventoryHandler, IEnumerable<IItemScript> itemScripts) : base("Inventory")
        {
            _inventoryHandler = inventoryHandler;

            //itemScripts.FirstOrDefault(i => i.ScriptName == "vest_itemscript").UseItem();
        }

        public InventoryTypes GetInventoryType(Position position, DimensionTypes dimensionType)
        {
            if (dimensionType == DimensionTypes.WORLD)
            {
                IVehicle vehicle = Alt.GetAllVehicles().FirstOrDefault(v => v.Position.Distance(position) < 2.5);
                if (vehicle != null) return InventoryTypes.VEHICLE;

                return InventoryTypes.PLAYER;
            }
            else if (dimensionType == DimensionTypes.TEAMHOUSE)
            {
                return InventoryTypes.TEAMHOUSE;
            }

            return InventoryTypes.PLAYER;
        }

        public async void OpenInventory(PXPlayer player)
        {
            await _inventoryHandler.LoadInventory(GetInventoryType(player.Position, player.DimensionType), 1);
        }
    }
}
