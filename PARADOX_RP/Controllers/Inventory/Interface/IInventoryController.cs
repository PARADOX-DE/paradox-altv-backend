using PARADOX_RP.Game.Inventory;
using PARADOX_RP.Game.Inventory.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Controllers.Inventory
{
    interface IInventoryController
    {
        Task LoadInventory(InventoryTypes type, int Id);
        Task CreateItem(int Id, IItem item, string OriginInformation, [CallerMemberName] string callerName = null);
    }
}
