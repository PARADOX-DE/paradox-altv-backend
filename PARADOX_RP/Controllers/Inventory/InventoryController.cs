using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Async.Elements.Refs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Game.Inventory;
using PARADOX_RP.Game.Inventory.Interfaces;
using PARADOX_RP.Game.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Controllers.Inventory
{
    public class InventoryController : IInventoryController
    {
        private readonly Dictionary<int, PXInventory> _inventories = new Dictionary<int, PXInventory>();

        public async Task<PXInventory> CreateInventory(InventoryTypes type, int Id)
        {
            await using (var px = new PXContext())
            {
                await px.Inventories.AddAsync(new Inventories() { Type = type, TargetId = Id });
                await px.SaveChangesAsync();
            }

            return await LoadInventory(type, Id);
        }

        public async Task<PXInventory> LoadInventory(InventoryTypes type, int Id)
        {
            await using (var px = new PXContext())
            {
                Inventories dbInventory = await px.Inventories.Include(i => i.Items)
                                                            .Where(p => p.Type == type && p.TargetId == Id).FirstOrDefaultAsync();
                if (dbInventory == null)
                {
                    //TODO: add logger
                    return null;
                }

                if (!InventoryModule.Instance._inventoryInfo.TryGetValue((int)dbInventory.Type, out InventoryInfo inventoryInfo))
                {
                    //Inventory got no valid type
                    return null;
                }

                PXInventory inventory = new PXInventory()
                {
                    Id = dbInventory.Id,
                    InventoryInfo = inventoryInfo,
                    TargetId = dbInventory.TargetId
                };

                /** IF INVENTORY IS VEHICLE ADJUST SLOTS **/

                foreach (InventoryItemAssignments item in dbInventory.Items)
                {
                    inventory.Items.Add(item.Slot, item);
                }

                _inventories.Add(inventory.Id, inventory);

                return inventory;
            }
        }

        public void UnloadInventory(int InventoryId)
        {
            if (_inventories.ContainsKey(InventoryId)) _inventories.Remove(InventoryId);
        }


        public int GetNextAvailableSlot(PXInventory inventory)
        {
            /*if (!InventoryModule.Instance._inventoryInfo.TryGetValue((int)inventory.Type, out InventoryInfo inventoryInfo)) return -1;

            for (int i = 1; i < inventoryInfo.MaxSlots; i++)
            {
                if (inventory.Items.FirstOrDefault(item => item.Slot == i) == null)
                {
                    return i;
                }
            }

            return -1;*/
            return 0;
        }

        public async Task<int> CreateItemSignature(string CallerName, string OriginInformation, int Amount)
        {
            await using (var px = new PXContext())
            {
                EntityEntry<InventoryItemSignatures> itemSignature = await px.InventoryItemSignatures.AddAsync(new InventoryItemSignatures()
                {
                    Origin = CallerName,
                    Information = OriginInformation,
                    Amount = Amount
                });
                await px.SaveChangesAsync();

                return itemSignature.Entity.Id;
            }
        }

        public async Task<bool> CreateItem(PXInventory inventory, int ItemId, int Amount, string OriginInformation, [CallerMemberName] string callerName = null)
        {
            if (!InventoryModule.Instance._items.TryGetValue(ItemId, out Items Item)) return false;
            if (Amount < 1) return false;

            var localItems = inventory.Items.Where(d => (d.Value.Item == ItemId) && (d.Value.Amount < Item.StackSize)).ToDictionary(pair => pair.Key).Values;
            int toBeAdded = Amount;

            int OriginId = await CreateItemSignature(callerName, OriginInformation, Amount);

            foreach (var localItem in localItems)
            {
                int oldAmount = localItem.Value.Amount;
                int newAmount = localItem.Value.Amount += toBeAdded;

                int NewOriginId = -1;
                if (newAmount > localItem.Value.Amount)
                    NewOriginId = OriginId;

                newAmount = newAmount >= Item.StackSize ? Item.StackSize : newAmount;
                localItem.Value.Amount = newAmount;
                toBeAdded -= (newAmount - oldAmount);

                await ChangeAmount(inventory, localItem.Key, newAmount, NewOriginId);

                if (toBeAdded <= 0) return true;
            }

            for (int i = 1; i < inventory.InventoryInfo.MaxSlots; i++)
            {
                if (inventory.Items.Keys.Contains(i)) continue;

                var newItem = new InventoryItemAssignments()
                {
                    InventoryId = inventory.Id,
                    OriginId = OriginId,
                    Item = ItemId,
                    Weight = Item.Weight,
                    Amount = toBeAdded >= Item.StackSize ? Item.StackSize : toBeAdded,
                    Slot = i
                };

                toBeAdded -= Item.StackSize;
                inventory.Items.Add(i, newItem);

                await using (var px = new PXContext())
                {
                    await px.InventoryItemAssignments.AddAsync(newItem);
                    await px.SaveChangesAsync();
                }

                if (toBeAdded <= 0)
                    return true;
            }
            return false;
        }

        public async Task RemoveItemBySlotId(PXInventory inventory, int SlotId, int Amount)
        {
            if (!inventory.Items.TryGetValue(SlotId, out InventoryItemAssignments Item)) return;
            if (!InventoryModule.Instance._items.TryGetValue(Item.Item, out Items ItemInfo)) return;

            if (Item.Amount < 1) return; // ggf. Logs hinzufügen.
            if (Item.Amount > ItemInfo.StackSize) return; // ebenfalls ggf. Logs

            if (Item.Amount <= Amount)
            {
                inventory.Items.Remove(SlotId);
                await using (var px = new PXContext())
                {
                    InventoryItemAssignments dbItem = await px.InventoryItemAssignments.FirstOrDefaultAsync(i => i.InventoryId == inventory.Id && i.Slot == SlotId);
                    if (dbItem == null) return;

                    px.InventoryItemAssignments.Remove(dbItem);
                    await px.SaveChangesAsync();
                }

                // Item komplett entfernen
            }
            else
            {
                // Item Anzahl entfernen
                Item.Amount -= Amount;
                await ChangeAmount(inventory, SlotId, Item.Amount);
            }

        }

        public async Task ChangeAmount(PXInventory inventory, int Slot, int Amount, int NewSignatureId = -1)
        {
            await using (var px = new PXContext())
            {
                InventoryItemAssignments item = await px.InventoryItemAssignments.FirstOrDefaultAsync(i => i.InventoryId == inventory.Id && i.Slot == Slot);
                if (item == null) return;

                item.Amount = Amount;
                if (NewSignatureId > 0) item.OriginId = NewSignatureId;

                px.InventoryItemAssignments.Update(item);

                await px.SaveChangesAsync();
            }
        }

        public async Task<bool> UseItem(PXPlayer player, PXInventory inventory, int Slot)
        {
            if (!inventory.Items.TryGetValue(Slot, out InventoryItemAssignments Item)) return false;
            if (!InventoryModule.Instance._items.TryGetValue(Item.Item, out Items ItemInfo)) return false;

            //itemScripts.FirstOrDefault(i => i.ScriptName == "vest_itemscript").UseItem();
            IItemScript TargetItemScript = InventoryModule.Instance._itemScripts.FirstOrDefault(i => i.ScriptName == ItemInfo.ScriptName);
            if (TargetItemScript == null) return false;

            bool NeedToRemoveItem = await TargetItemScript.UseItem(player);
            if (NeedToRemoveItem)
            {
                //Remove Item Logic here
                if (new AsyncPlayerRef(player).Exists)
                    await RemoveItemBySlotId(inventory, Slot, 1);
            }

            return false;
        }
    }
}
