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
        private IInventoryController _inventoryHandler;

        private IEnumerable<IInventoriable> _inventories;
        public Dictionary<int, Items> _items = new Dictionary<int, Items>();
        public Dictionary<int, InventoryInfo> _inventoryInfo = new Dictionary<int, InventoryInfo>();

        public InventoryModule(PXContext pxContext, IInventoryController inventoryHandler, IEnumerable<IInventoriable> inventories, IEnumerable<IItemScript> itemScripts) : base("Inventory")
        {
            _inventoryHandler = inventoryHandler;
            _inventories = inventories;

            LoadDatabaseTable<Items>(pxContext.Items, (i) => _items.Add(i.Id, i));
            LoadDatabaseTable<InventoryInfo>(pxContext.InventoryInfo, (i) => _inventoryInfo.Add((int)i.InventoryType, i));
            //itemScripts.FirstOrDefault(i => i.ScriptName == "vest_itemscript").UseItem();
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

        public async Task<InventoryTypes> GetInventoryType(PXPlayer player, Position position)
        {
            InventoryTypes inventoryType = InventoryTypes.PLAYER;

            await _inventories.ForEach(async (i) =>
            {
                InventoryTypes type = await i.OnInventoryOpen(player, position);
                if(type != InventoryTypes.PLAYER)
                {
                    inventoryType = type;
                    return;
                }
            });

            return inventoryType;
        }

        public async Task OpenInventory(PXPlayer player)
        {
            Inventories inventory = player.Inventory;
            if (inventory == null) return;

            AltAsync.Log(Enum.GetName(typeof(InventoryTypes), await GetInventoryType(player, player.Position)));
            GetInventoryType(player, player.Position)

            WindowManager.Instance.Get<InventoryWindow>().Show(player, new InventoryWindowWriter(player.Inventory, null));
        }

        public bool HasItem(Inventories inventory, int ItemId)
        {
            if (inventory == null) return false;

            return inventory.Items.FirstOrDefault(i => i.Item == ItemId) != null;
        }
    }
}
