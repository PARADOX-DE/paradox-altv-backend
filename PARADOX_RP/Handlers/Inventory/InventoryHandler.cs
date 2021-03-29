using AltV.Net;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Game.Inventory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Handlers.Inventory
{
    class InventoryHandler : IInventoryHandler
    {
        public async Task LoadInventory(InventoryTypes type, int Id)
        {
            using (var px = new PXContext())
            {
                Inventories inventory = await px.Inventories.Include(i => i.Items).FirstOrDefaultAsync(p => p.Type == type && p.TargetId == Id);
                foreach (InventoryItemAssignments item in inventory.Items)
                    Alt.Log(item.Item);
            }
        }
    }
}
