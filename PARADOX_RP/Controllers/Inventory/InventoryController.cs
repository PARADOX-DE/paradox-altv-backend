using AltV.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Game.Inventory;
using PARADOX_RP.Game.Inventory.Interfaces;
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

        public async Task<Inventories> CreateInventory(InventoryTypes type, int Id)
        {
            await using (var px = new PXContext())
            {
                await px.Inventories.AddAsync(new Inventories() { Type = InventoryTypes.VEHICLE, TargetId = Id });
                await px.SaveChangesAsync();

                var dbInventory = await px.Inventories.Include(i => i.Items).Where(p => p.Type == type && p.TargetId == Id).FirstOrDefaultAsync();
                return dbInventory;
            }
        }

        public async Task<Inventories> LoadInventory(InventoryTypes type, int Id)
        {
            await using (var px = new PXContext())
            {
                Inventories inventory = await px.Inventories.Include(i => i.Items).Where(p => p.Type == type && p.TargetId == Id).FirstOrDefaultAsync();
                if (inventory == null)
                {
                    //TODO: add logger
                    return null;
                }

                return inventory;
            }
        }

        public async Task CreateItem(Inventories inventory, int ItemId, string OriginInformation, [CallerMemberName] string callerName = null)
        {
            await using (var px = new PXContext())
            {
                EntityEntry<InventoryItemSignatures> itemSignature = await px.InventoryItemSignatures.AddAsync(new InventoryItemSignatures()
                {
                    Origin = callerName,
                    Information = OriginInformation
                });

                await px.SaveChangesAsync();

                //TODO: item signatures system change
                
                await px.InventoryItemAssignments.AddAsync(new InventoryItemAssignments()
                {
                    InventoryId = inventory.Id,
                    OriginId = itemSignature.Entity.Id,
                    Item = ItemId,
                    Weight = 0,
                    Slot = 0
                });
                await px.SaveChangesAsync();
            }
        }
    }
}
