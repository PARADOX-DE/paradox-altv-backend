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
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Controllers.Inventory
{
    class InventoryController : IInventoryController
    {
        public async Task LoadInventory(InventoryTypes type, int Id)
        {
            await using (var px = new PXContext())
            {
                Inventories inventory = await px.Inventories.Include(i => i.Items).FirstOrDefaultAsync(p => p.Type == type && p.TargetId == Id);
                foreach (InventoryItemAssignments item in inventory.Items)
                    Alt.Log(item.Item.ToString());
            }
        }

        public async Task CreateItem(int Id, IItemScript item, string OriginInformation, [CallerMemberName] string callerName = null)
        {
            await using(var px = new PXContext())
            {
                EntityEntry<InventoryItemSignatures> itemSignature = await px.InventoryItemSignatures.AddAsync(new InventoryItemSignatures()
                {
                    Origin = callerName,
                    Information = OriginInformation
                });

                await px.InventoryItemAssignments.AddAsync(new InventoryItemAssignments()
                {
                    InventoryId = Id,
                    OriginId = itemSignature.Entity.Id,
                    Item = item.Id,
                    Weight = 0,
                    Slot = 0
                });

                await px.SaveChangesAsync();
            }
        }
    }
}
