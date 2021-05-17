using AltV.Net;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.UI.Windows
{
    class GarageWindow : Window
    {
        public GarageWindow() : base("Garage") { }
    }

    class GarageWindowWriter : IWritable
    {
        public GarageWindowWriter(int id, string garageName, IEnumerable<Vehicles> vehicles)
        {
            Id = id;
            GarageName = garageName;
            Vehicles = vehicles;
        }

        private int Id { get; set; }
        private string GarageName { get; set; }
        private IEnumerable<Vehicles> Vehicles { get; set; }

        public void OnWrite(IMValueWriter writer)
        {
            writer.BeginObject();
                writer.Name("id");
                writer.Value(Id);
                writer.Name("garageName");
                writer.Value(GarageName);

                writer.Name("vehicles");
                writer.BeginArray();
                if(Vehicles != null)
                    foreach(Vehicles vehicle in Vehicles)
                    {
                        writer.BeginObject();
                            writer.Name("id");
                            writer.Value(vehicle.Id);
                            writer.Name("model");
                            writer.Value(vehicle.VehicleModel);
                        writer.EndObject();
                    }
                writer.EndArray();
            writer.EndObject();
        }
    }
}
