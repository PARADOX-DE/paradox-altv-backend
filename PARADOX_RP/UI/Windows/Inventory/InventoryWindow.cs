using AltV.Net;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Game.Inventory;
using PARADOX_RP.Game.Inventory.Models;
using PARADOX_RP.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.UI.Windows.Inventory
{
    class InventoryWindow : Window
    {
        public InventoryWindow() : base("Inventory") { }

    }
    class InventoryWindowWriter : IWritable
    {
        private readonly PXInventory playerInventory;
        private readonly PXInventory additionalInventory;

        public InventoryWindowWriter(PXInventory playerInventory, PXInventory additionalInventory)
        {
            this.playerInventory = playerInventory;
            this.additionalInventory = additionalInventory;
        }

        public void OnWrite(IMValueWriter writer)
        {
            writer.BeginObject();

            WriteInventory("player", ref writer, playerInventory);
            WriteInventory("additional", ref writer, additionalInventory);

            writer.EndObject();
        }

        private void WriteInventory(string name, ref IMValueWriter writer, PXInventory inventory)
        {
            if (inventory == null) return;

            writer.Name(name);
            writer.BeginObject();

            writer.Name("items");
            writer.BeginArray();
            foreach (var item in inventory.Items)
            {
                if (InventoryModule.Instance._items.TryGetValue(item.Value.Item, out Items itemInfo))
                {
                    writer.BeginObject();

                    writer.Name("id");
                    writer.Value(item.Value.Id);

                    writer.Name("name");
                    writer.Value(itemInfo.Name);

                    writer.Name("amount");
                    writer.Value(1);

                    writer.Name("weight");
                    writer.Value(itemInfo.Weight);

                    writer.Name("slot");
                    writer.Value(item.Key);

                    //TODO: add reference to items db

                    writer.EndObject();
                }
            }
            writer.EndArray();

            if (inventory.InventoryInfo != null)
            {
                writer.Name("title");
                writer.Value(inventory.InventoryInfo.Name);

                writer.Name("max_slots");
                writer.Value(inventory.InventoryInfo.MaxSlots);

                writer.Name("max_weight");
                writer.Value(inventory.InventoryInfo.MaxWeight);
            }

            writer.EndObject();
        }
    }
}
