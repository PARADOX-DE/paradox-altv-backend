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
        Task<bool> CreateItem(PXInventory inventory, int ItemId, int Amount, string OriginInformation, [CallerMemberName] string callerName = null);
        Task RemoveItemBySlotId(PXInventory inventory, int ItemId, int Amount);
        int GetNextAvailableSlot(PXInventory inventory);
        Task<int> CreateItemSignature(string CallerName, string OriginInformation);
        
        Task<bool> UseItem(PXInventory inventory, int Slot);
    }
}
