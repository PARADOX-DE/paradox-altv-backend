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
    interface IInventoryController
    {
        Task<Inventories> LoadInventory(InventoryTypes type, int Id);
        Task<Inventories> CreateInventory(InventoryTypes type, int Id);
        Task CreateItem(int Id, IItemScript item, string OriginInformation, [CallerMemberName] string callerName = null);
    }
}
