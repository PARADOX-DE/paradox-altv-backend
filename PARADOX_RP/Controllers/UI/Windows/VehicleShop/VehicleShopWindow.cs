using AltV.Net;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.UI.Windows.CarShop
{
    class VehicleShopWindow : Window
    {
        public VehicleShopWindow() : base("CarShop") { }
    }

    class VehicleShopWindowWriter : IWritable
    {
        private int Id;
        private List<VehicleShopsContent> Content;

        public VehicleShopWindowWriter(int id, List<VehicleShopsContent> content)
        {
            Id = id;
            Content = content;
        }

        public void OnWrite(IMValueWriter writer)
        {
            writer.BeginObject();
            writer.Name("Id");
            writer.Value(Id);
            writer.Name("Content");
            writer.BeginArray();
            if (Content != null)
                foreach (var item in Content)
                {
                    writer.BeginObject();

                    writer.Name("name");
                    writer.Value(item.VehicleClass.VehicleModel);
                    writer.Name("fuel");
                    writer.Value(item.VehicleClass.MaxFuel);
                    writer.Name("maxspeed");
                    writer.Value(item.VehicleClass.MaxSpeed);
                    writer.Name("rate");
                    writer.Value(item.VehicleClass.FuelConsumption);
                    writer.Name("trunk");
                    writer.Value(item.VehicleClass.InvWeight);
                    writer.Name("price");
                    writer.Value(item.Price);

                    writer.EndObject();
                }
            writer.EndArray();
            writer.EndObject();
        }
    }
}
