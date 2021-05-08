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
        private readonly ICollection<InventoryItemAssignments> inventoryItemAssignments;

        public InventoryWindowWriter(ICollection<InventoryItemAssignments> inventoryItemAssignments)
        {
            this.inventoryItemAssignments = inventoryItemAssignments;
        }

        public void OnWrite(IMValueWriter writer)
        {
            writer.BeginArray();
            foreach (var item in inventoryItemAssignments)
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
        }
    }
}
