using AltV.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
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
    class InventoryController : IInventoryController
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

                if(!InventoryModule.Instance._inventoryInfo.TryGetValue((int)dbInventory.Type, out InventoryInfo inventoryInfo))
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

                foreach(InventoryItemAssignments item in dbInventory.Items)
                {
                    inventory.Items.Add(item.Slot, item);
                }

                _inventories.Add(inventory.Id, inventory);

                return inventory;
            }
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

        public async Task CreateItem(PXInventory inventory, int ItemId, string OriginInformation, [CallerMemberName] string callerName = null)
        {
            if (!InventoryModule.Instance._items.TryGetValue(ItemId, out Items Item)) return;

            await using (var px = new PXContext())
            {
                EntityEntry<InventoryItemSignatures> itemSignature = await px.InventoryItemSignatures.AddAsync(new InventoryItemSignatures()
                {
                    Origin = callerName,
                    Information = OriginInformation
                });

                await px.SaveChangesAsync();

                //TODO: item signatures system change
                var newItem = new InventoryItemAssignments()
                {
                    InventoryId = inventory.Id,
                    OriginId = itemSignature.Entity.Id,
                    Item = ItemId,
                    Weight = Item.Weight,
                    Slot = GetNextAvailableSlot(inventory)
                };

                await px.InventoryItemAssignments.AddAsync(newItem);
                await px.SaveChangesAsync();

                //inventory.Items.Add(newItem);
            }
        }
    }
}
