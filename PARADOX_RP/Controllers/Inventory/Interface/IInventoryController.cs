using PARADOX_RP.Game.Inventory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Controllers.Inventory
{
    interface IInventoryController
    {
        Task LoadInventory(InventoryTypes type, int Id);
    }
}
