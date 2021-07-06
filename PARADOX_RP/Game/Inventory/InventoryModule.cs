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
using PARADOX_RP.Core.Events;

namespace PARADOX_RP.Game.Inventory
{
    public enum InventoryTypes
    {
        PLAYER,
        VEHICLE,
        CRYPTOROOM,
        TEAMHOUSE,
        LOCKER,
        EVENT
    }

    class InventoryModule : Module<InventoryModule>, IEventKeyPressed
    {
        private IInventoryController _inventoryHandler;

        private IEnumerable<IInventoriable> _inventories;
        public IEnumerable<IItemScript> _itemScripts;

        public Dictionary<int, Items> _items = new Dictionary<int, Items>();
        public Dictionary<int, InventoryInfo> _inventoryInfo = new Dictionary<int, InventoryInfo>();

        public InventoryModule(PXContext pxContext, IInventoryController inventoryHandler, IEventController eventController, IEnumerable<IInventoriable> inventories, IEnumerable<IItemScript> itemScripts) : base("Inventory")
        {
            _inventoryHandler = inventoryHandler;
            _inventories = inventories;
            _itemScripts = itemScripts;

            LoadDatabaseTable<Items>(pxContext.Items, (i) => _items.Add(i.Id, i));
            LoadDatabaseTable<InventoryInfo>(pxContext.InventoryInfo, (i) => _inventoryInfo.Add((int)i.InventoryType, i));
            //itemScripts.FirstOrDefault(i => i.ScriptName == "vest_itemscript").UseItem();

            eventController.OnClient<PXPlayer, int, int, int, bool, bool>("MoveInventoryItem", MoveInventoryItem);
            eventController.OnClient<PXPlayer, int>("UseItem", UseItem);
        }

        public async Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key)
        {
            if (key == KeyEnumeration.I)
            {
                await OpenInventory(player);
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        private async void UseItem(PXPlayer player, int Slot)
        {
            LocalInventoryData localInventoryData = player.LocalInventoryData;
            if (localInventoryData == null) return;

            await _inventoryHandler.UseItem(player, localInventoryData.PlayerInventory, Slot);
        }

        private async void MoveInventoryItem(PXPlayer player, int oldSlot, int newSlot, int Amount, bool ToAdditional, bool FromAdditional)
        {
            LocalInventoryData localInventoryData = player.LocalInventoryData;
            if (localInventoryData == null) return;

            // idk if its good, maybe rework some days 

            if (localInventoryData.AdditionalInventory == null && (ToAdditional || FromAdditional))
            {
                // Interaktion mit 2. Inventar, aber Inventar existiert nicht.
                return;
            }

            if (FromAdditional || ToAdditional)
            {
                bool? CanAccess = await CanAccessInventory(localInventoryData.AdditionalInventory, player);
                if (CanAccess != true)
                {
                    // AdditionalInventory ist nicht mehr zugreifbar.
                    player.SendNotification("Inventar", "Auf dieses Inventar ist nicht mehr zugreifbar.", NotificationTypes.ERROR);
                    WindowController.Instance.Get<InventoryWindow>().Hide(player);
                    return;
                }
            }

            PXInventory fromInventory = FromAdditional ? localInventoryData.AdditionalInventory : localInventoryData.PlayerInventory;
            //var fromInventoryFreeWeight = GetFreeWeight(fromInventory);
            if (!fromInventory.Items.TryGetValue(oldSlot, out InventoryItemAssignments targetItem))
            {
                // Item welches verschoben wird existiert nicht
                WindowController.Instance.Get<InventoryWindow>().Hide(player);
                return;
            }

            PXInventory targetMoveInventory = ToAdditional ? localInventoryData.AdditionalInventory : localInventoryData.PlayerInventory;
            var targetMoveInventoryFreeWeight = GetFreeWeight(targetMoveInventory);
            if (targetMoveInventory.Items.TryGetValue(newSlot, out _))
            {
                // Auf dem Slot liegt bereits ein Item
                WindowController.Instance.Get<InventoryWindow>().Hide(player);
                return;
            }

            if (targetItem.Amount < Amount || targetItem.Amount < 1)
            {
                // Spieler verschiebt mehr als vorhanden. / Invalid item
                return;
            }

            if ((targetMoveInventoryFreeWeight + targetItem.Weight) >= targetMoveInventory.InventoryInfo.MaxWeight)
            {
                // Kein Platz für das Item
                player.SendNotification("Inventar", "Du hast nicht genügend Platz.", NotificationTypes.ERROR);
                WindowController.Instance.Get<InventoryWindow>().Hide(player);
                return;
            }

            if (targetItem.Amount == Amount)
            {
                // Volle Anzahl wird verschoben
                fromInventory.Items.Remove(oldSlot);
                targetItem.Slot = newSlot;

                targetMoveInventory.Items.Add(newSlot, targetItem);

                await using var px = new PXContext();
                var dbTargetItem = await px.InventoryItemAssignments.FindAsync(targetItem.Id);
                if (dbTargetItem == null) return;

                dbTargetItem.Slot = newSlot;
                dbTargetItem.InventoryId = targetMoveInventory.Id;
                await px.SaveChangesAsync();
            }
            else
            {
                targetItem.Amount -= Amount;
                if (_items.TryGetValue(targetItem.Item, out Items targetItemInfo))
                {
                    if (targetItem.Amount < 1 || targetItem.Amount > targetItemInfo.StackSize)
                    {
                        // Item invalid, log
                        return;
                    }

                    var newItem = new InventoryItemAssignments()
                    {
                        InventoryId = targetMoveInventory.Id,
                        OriginId = targetItem.OriginId,
                        Item = targetItem.Item,
                        Weight = targetItem.Weight,
                        Amount = Amount,
                        Slot = newSlot
                    };

                    targetMoveInventory.Items.Add(newSlot, newItem);

                    await using (var px = new PXContext())
                    {
                        await px.InventoryItemAssignments.AddAsync(newItem);
                        await px.SaveChangesAsync();
                    }
                }
            }

            //TODO: animation library
            await player.PlayAnimation("mp_safehousevagos@", "package_dropoff", 9, 2000);
        }

        public async Task<PXInventory> GetAdditionalInventory(PXPlayer player, Position position)
        {
            PXInventory inventory = null;

            await _inventories.ForEach(async (i) =>
            {
                PXInventory additionalInventory = await i.OnInventoryOpen(player, position);
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
            PXInventory inventory = player.Inventory;
            if (inventory == null) return;

            PXInventory additionalInventory = await GetAdditionalInventory(player, player.Position);

            player.LocalInventoryData = new LocalInventoryData(player.Inventory, additionalInventory);

            WindowController.Instance.Get<InventoryWindow>().Show(player, new InventoryWindowWriter(player.Inventory, additionalInventory));
        }

        public bool HasItem(PXInventory inventory, int ItemId)
        {
            if (inventory == null) return false;

            return inventory.Items.Values.FirstOrDefault(i => i.Item == ItemId) != null;
        }

        public async Task<bool?> CanAccessInventory(PXInventory inventory, PXPlayer player)
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

        public bool VerifyItem(PXInventory inventory, int itemObjectId)
        {
            var targetItem = inventory.Items.FirstOrDefault(i => i.Value.Id == itemObjectId);
            if (targetItem.Value == null) return false;

            if (targetItem.Key < 0 || targetItem.Key > inventory.InventoryInfo.MaxSlots) return false;

            InventoryItemSignatures targetSignature = targetItem.Value.Origin;
            if (targetSignature == null) return false;

            if (targetSignature.Amount < targetItem.Value.Amount) return false;

            return true;
        }

        public int GetUsedWeight(PXInventory inventory)
        {
            int usedWeight = 0;
            foreach (var item in inventory.Items.Values)
                if (_items.TryGetValue(item.Item, out Items itemData))
                    usedWeight += item.Amount * itemData.Weight;

            return usedWeight;
        }

        public int GetFreeWeight(PXInventory inventory)
        {
            return inventory.InventoryInfo.MaxWeight - GetUsedWeight(inventory);
        }

        public bool CanAddItem(PXInventory inventory, int itemId, int amount = 1)
        {
            if (!_items.TryGetValue(itemId, out Items itemData)) return false;

            var localItems = inventory.Items.Values.Where(d => d.Item == itemId);

            int toBeAdded = amount;

            foreach (var localItem in localItems)
            {
                toBeAdded -= itemData.StackSize - localItem.Amount;

                if (toBeAdded <= 0) break;
            }

            if (toBeAdded > 0)
            {
                int freeSlots = inventory.InventoryInfo.MaxSlots - inventory.Items.Count;
                if (freeSlots <= 0) return false;

                if (freeSlots < toBeAdded / itemData.StackSize)
                {
                    return false;
                }
            }

            return itemData.Weight * amount <= GetFreeWeight(inventory);
        }
    }
}