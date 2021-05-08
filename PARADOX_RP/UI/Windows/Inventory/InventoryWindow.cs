using AltV.Net;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Game.Inventory;
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
        private readonly Inventories playerInventory;
        private readonly Inventories additionalInventory;

        public InventoryWindowWriter(Inventories playerInventory, Inventories additionalInventory)
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

        private void WriteInventory(string name, ref IMValueWriter writer, Inventories inventory)
        {
            if (inventory == null) return;

            writer.Name(name);
            writer.BeginObject();

            writer.Name("items");
            writer.BeginArray();
            foreach (var item in inventory.Items)
            {
                if (InventoryModule.Instance._items.TryGetValue(item.Id, out Items itemInfo))
                {

                    writer.BeginObject();

                    writer.Name("id");
                    writer.Value(item.Id);

                    writer.Name("name");
                    writer.Value(itemInfo.Name);

                    writer.Name("amount");
                    writer.Value(1);

                    writer.Name("weight");
                    writer.Value(itemInfo.Weight);

                    writer.Name("slot");
                    writer.Value(item.Id);
                    //TODO: slot ;)

                    //TODO: add reference to items db

                    writer.EndObject();
                }
            }
            writer.EndArray();

            if (InventoryModule.Instance._inventoryInfo.TryGetValue((int)inventory.Type, out InventoryInfo inventoryInfo))
            {
                writer.Name("title");
                writer.Value(inventoryInfo.Name);

                writer.Name("max_slots");
                writer.Value(inventoryInfo.MaxSlots);

                writer.Name("max_weight");
                writer.Value(inventoryInfo.MaxWeight);
            }

            writer.EndObject();
        }
    }
}
