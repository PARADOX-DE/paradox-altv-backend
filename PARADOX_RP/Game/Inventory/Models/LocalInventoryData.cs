using PARADOX_RP.Core.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Inventory.Models
{
    public class LocalInventoryData
    {
        public LocalInventoryData(PXInventory playerInventory, PXInventory additionalInventory)
        {
            PlayerInventory = playerInventory;
            AdditionalInventory = additionalInventory;
        }

        public PXInventory PlayerInventory { get; set; }
        public PXInventory AdditionalInventory { get; set; }
    }
}
