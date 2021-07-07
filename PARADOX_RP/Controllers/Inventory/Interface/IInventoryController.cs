using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
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
    public interface IInventoryController
    {
        Task<PXInventory> LoadInventory(InventoryTypes type, int Id);
        void UnloadInventory(int InventoryId);
        Task<PXInventory> CreateInventory(InventoryTypes type, int Id);
        Task<bool> CreateItem(PXInventory inventory, int ItemId, int Amount, string OriginInformation, [CallerMemberName] string callerName = null);
        Task RemoveItemBySlotId(PXInventory inventory, int SlotId, int Amount);
        int GetNextAvailableSlot(PXInventory inventory);
        Task<int> CreateItemSignature(string CallerName, string OriginInformation, int Amount);
        
        Task<bool> UseItem(PXPlayer player, PXInventory inventory, int Slot);
    }
}
