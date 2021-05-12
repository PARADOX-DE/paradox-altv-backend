using PARADOX_RP.Core.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Inventory.Models
{
    public class LocalInventoryData
    {
        public LocalInventoryData(Inventories playerInventory, Inventories additionalInventory)
        {
            PlayerInventory = playerInventory;
            AdditionalInventory = additionalInventory;
        }

        public Inventories PlayerInventory { get; set; }
        public Inventories AdditionalInventory { get; set; }
    }
}
