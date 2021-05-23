using AltV.Net;
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
using PARADOX_RP.Core.Database.Models;
using System.Threading.Tasks;
using PARADOX_RP.Utils.Enums;
using Newtonsoft.Json;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Database;
using PARADOX_RP.UI;
using PARADOX_RP.UI.Windows.Inventory;
using AltV.Net.Async;
using PARADOX_RP.Game.Inventory.Models;

namespace PARADOX_RP.Game.Inventory
{
    public enum InventoryTypes
    {
        PLAYER,
        VEHICLE,
        TEAMHOUSE,
        STORAGEROOM,
        LOCKER,
        EVENT
    }

    class InventoryModule : ModuleBase<InventoryModule>
    {
        private IInventoryController _inventoryHandler;

        private IEnumerable<IInventoriable> _inventories;
        public Dictionary<int, Items> _items = new Dictionary<int, Items>();
        public Dictionary<int, InventoryInfo> _inventoryInfo = new Dictionary<int, InventoryInfo>();

        public InventoryModule(PXContext pxContext, IInventoryController inventoryHandler, IEventController eventController, IEnumerable<IInventoriable> inventories, IEnumerable<IItemScript> itemScripts) : base("Inventory")
        {
            _inventoryHandler = inventoryHandler;
            _inventories = inventories;

            LoadDatabaseTable<Items>(pxContext.Items, (i) => _items.Add(i.Id, i));
            LoadDatabaseTable<InventoryInfo>(pxContext.InventoryInfo, (i) => _inventoryInfo.Add((int)i.InventoryType, i));
            //itemScripts.FirstOrDefault(i => i.ScriptName == "vest_itemscript").UseItem();

            eventController.OnClient<PXPlayer, int, int, int, bool>("MoveInventoryItem", MoveInventoryItem);
        }

        public override async Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key)
        {
            if (key == KeyEnumeration.I)
            {
                await OpenInventory(player);
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        private void MoveInventoryItem(PXPlayer player, int OldSlot, int NewSlot, int Amount, bool ToAdditional)
        {
            LocalInventoryData localInventoryData = player.LocalInventoryData;
            if (localInventoryData == null) return;

        }

        public async Task<Inventories> GetAdditionalInventory(PXPlayer player, Position position)
        {
            Inventories inventory = null;

            await _inventories.ForEach(async (i) =>
            {
                Inventories additionalInventory = await i.OnInventoryOpen(player, position);
                if (additionalInventory != null)
                {
                    inventory = additionalInventory;
                    return;
                }
            });

            return inventory;
        }

        public async Task OpenInventory(PXPlayer player)
        {
            Inventories inventory = player.Inventory;
            if (inventory == null) return;

            Inventories additionalInventory = await GetAdditionalInventory(player, player.Position);
            if (additionalInventory != null)
            {
                AltAsync.Log("OPENINV: " + Enum.GetName(typeof(InventoryTypes), additionalInventory.Type));
            }

            player.LocalInventoryData = new LocalInventoryData(player.Inventory, additionalInventory);

            WindowManager.Instance.Get<InventoryWindow>().Show(player, new InventoryWindowWriter(player.Inventory, additionalInventory));
        }

        public bool HasItem(Inventories inventory, int ItemId)
        {
            if (inventory == null) return false;

            return inventory.Items.FirstOrDefault(i => i.Item == ItemId) != null;
        }

        public async Task<bool?> CanAccessInventory(Inventories inventory, PXPlayer player)
        {
            bool? accessible = null;

            await _inventories.ForEach(async (i) =>
            {
                bool? _accessible = await i.CanAccessInventory(player, inventory);
                if (_accessible != null)
                {
                    accessible = _accessible;
                    return;
                }
            });

            return accessible;
        }
    }
}
