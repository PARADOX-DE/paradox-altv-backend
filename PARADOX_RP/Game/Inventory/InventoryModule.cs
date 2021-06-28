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

    class InventoryModule : ModuleBase<InventoryModule>, IEventKeyPressed
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

        private async void MoveInventoryItem(PXPlayer player, int OldSlot, int NewSlot, int Amount, bool ToAdditional, bool FromAdditional)
        {
            LocalInventoryData localInventoryData = player.LocalInventoryData;
            if (localInventoryData == null) return;

            // idk if its good, maybe rework some days 

            if (localInventoryData.AdditionalInventory == null && (ToAdditional || FromAdditional))
            {
                // Spieler will in 2. Inventar Item ziehen -> Aber Inventar existiert nicht.
                return;
            }

            if (FromAdditional || ToAdditional)
            {
                //Spieler will interagieren mit 2. Inventar
                bool? CanAccess = await CanAccessInventory(localInventoryData.AdditionalInventory, player);
                if (CanAccess != true)
                {
                    // Inventar ist nicht mehr zugreifbar
                    player.SendNotification("Inventar", "Auf dieses Inventar ist nicht mehr zugreifbar.", NotificationTypes.ERROR);
                    WindowController.Instance.Get<InventoryWindow>().Hide(player);
                    return;
                }

                // second inventory -> second inventory
                if (FromAdditional && ToAdditional)
                {
                    if (localInventoryData.AdditionalInventory.Items.TryGetValue(OldSlot, out InventoryItemAssignments OldSlotItem)) // prüfe ob auf alten Slot das Item verfügbar ist
                    {
                        if (OldSlotItem.Amount < 1) return;
                        if (localInventoryData.AdditionalInventory.Items.TryGetValue(NewSlot, out _)) return; //prüfe ob auf neuen Slot bereits ein Item ist

                        localInventoryData.AdditionalInventory.Items.ChangeKey(OldSlotItem.Slot, NewSlot);
                        OldSlotItem.Slot = NewSlot;

                        await using var px = new PXContext();
                        var dbItem = await px.InventoryItemAssignments.FindAsync(OldSlotItem.Id);
                        dbItem.Slot = NewSlot;
                        await px.SaveChangesAsync();
                    }
                }

                // second inventory -> main inventory
                if (FromAdditional && !ToAdditional)
                {
                    if (localInventoryData.AdditionalInventory.Items.TryGetValue(OldSlot, out InventoryItemAssignments OldSlotItem)) // prüfe ob auf alten Slot das Item verfügbar ist
                    {
                        if (OldSlotItem.Amount < 1) return;
                        if (localInventoryData.PlayerInventory.Items.TryGetValue(NewSlot, out _)) return; //prüfe ob auf neuen Slot bereits ein Item ist

                        localInventoryData.AdditionalInventory.Items.Remove(OldSlot);

                        OldSlotItem.Slot = NewSlot;
                        localInventoryData.PlayerInventory.Items.Add(OldSlotItem.Slot, OldSlotItem);

                        await using var px = new PXContext();
                        var dbItem = await px.InventoryItemAssignments.FindAsync(OldSlotItem.Id);
                        dbItem.Slot = NewSlot;
                        dbItem.InventoryId = localInventoryData.PlayerInventory.Id;
                        await px.SaveChangesAsync();
                    }
                }

                // main inventory -> second inventory
                if (!FromAdditional && ToAdditional)
                {
                    if (localInventoryData.PlayerInventory.Items.TryGetValue(OldSlot, out InventoryItemAssignments OldSlotItem)) // prüfe ob auf alten Slot das Item verfügbar ist
                    {
                        if (OldSlotItem.Amount < 1) return;
                        if (localInventoryData.AdditionalInventory.Items.TryGetValue(NewSlot, out _)) return; //prüfe ob auf neuen Slot bereits ein Item ist

                        localInventoryData.PlayerInventory.Items.Remove(OldSlot);

                        OldSlotItem.Slot = NewSlot;
                        localInventoryData.AdditionalInventory.Items.Add(OldSlotItem.Slot, OldSlotItem);

                        await using var px = new PXContext();
                        var dbItem = await px.InventoryItemAssignments.FindAsync(OldSlotItem.Id);
                        dbItem.Slot = NewSlot;
                        dbItem.InventoryId = localInventoryData.AdditionalInventory.Id;
                        await px.SaveChangesAsync();
                    }
                }
            }

            // main inventory -> main inventory
            if(!FromAdditional && !ToAdditional)
            {
                if (localInventoryData.PlayerInventory.Items.TryGetValue(OldSlot, out InventoryItemAssignments OldSlotItem)) // prüfe ob auf alten Slot das Item verfügbar ist
                {
                    if (OldSlotItem.Amount < 1) return;
                    if (localInventoryData.PlayerInventory.Items.TryGetValue(NewSlot, out _)) return; //prüfe ob auf neuen Slot bereits ein Item ist

                    localInventoryData.PlayerInventory.Items.ChangeKey(OldSlotItem.Slot, NewSlot);
                    OldSlotItem.Slot = NewSlot;

                    await using var px = new PXContext();
                    var dbItem = await px.InventoryItemAssignments.FindAsync(OldSlotItem.Id);
                    dbItem.Slot = NewSlot;
                    await px.SaveChangesAsync();
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
    }
}
