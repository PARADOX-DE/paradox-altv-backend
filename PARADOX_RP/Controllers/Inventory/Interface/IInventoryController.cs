using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Game.Inventory;
using PARADOX_RP.Game.Inventory.Interfaces;
using PARADOX_RP.Game.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Controllers.Inventory
{
    interface IInventoryController
    {
        Task<PXInventory> LoadInventory(InventoryTypes type, int Id);
        Task<PXInventory> CreateInventory(InventoryTypes type, int Id);
        Task CreateItem(PXInventory inventory, int ItemId, int Amount, string OriginInformation, [CallerMemberName] string callerName = nul);
        int GetNextAvailableSlot(PXInventory inventory);
    }
}
