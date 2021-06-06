using AltV.Net;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.UI.Windows
{
    class ShopWindow : Window
    {
        public ShopWindow() : base("Shop") { }
    }

    class ShopWindowWriter : IWritable
    {
        public ShopWindowWriter(int id, List<ShopItem> items)
        {
            Id = id;
            Items = items;
        }

        private int Id { get; set; }
        private List<ShopItem> Items { get; set; }

        public void OnWrite(IMValueWriter writer)
        {
            writer.BeginObject();
            writer.Name("id");
            writer.Value(Id);

            writer.Name("productList");
            writer.BeginArray();
            if (Items != null)
                foreach (ShopItem item in Items)
                {
                    writer.BeginObject();
                    writer.Name("id");
                    writer.Value(item.Id);
                    writer.Name("name");
                    writer.Value(item.Name);
                    writer.Name("price");
                    writer.Value(item.Price);
                    writer.EndObject();
                }
            writer.EndArray();

            writer.EndObject();
        }
    }
}
