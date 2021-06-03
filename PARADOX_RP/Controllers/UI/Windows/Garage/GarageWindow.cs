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

    class GarageWindowVehicle
    {
        public GarageWindowVehicle(int id, string model)
        {
            Id = id;
            VehicleModel = model;
        }

        public int Id { get; set; }
        public string VehicleModel { get; set; }
    }

    class GarageWindowWriter : IWritable
    {
        public GarageWindowWriter(int id, string garageName, List<GarageWindowVehicle> vehicles, GarageWindowVehicle nearestVehicle)
        {
            Id = id;
            GarageName = garageName;
            Vehicles = vehicles;
            NearestVehicle = nearestVehicle;
        }

        private int Id { get; set; }
        private string GarageName { get; set; }
        private List<GarageWindowVehicle> Vehicles { get; set; }
        private GarageWindowVehicle NearestVehicle { get; set; }

        public void OnWrite(IMValueWriter writer)
        {
            writer.BeginObject();
                writer.Name("id");
                writer.Value(Id);
                writer.Name("garageName");
                writer.Value(GarageName);

                writer.Name("nearest_vehicle");
                writer.BeginObject();
                    if(NearestVehicle != null) { 
                        writer.Name("id");
                        writer.Value(NearestVehicle.Id);
                        writer.Name("model");
                        writer.Value(NearestVehicle.VehicleModel);
                    }
                writer.EndObject();

                writer.Name("vehicles");
                writer.BeginArray();
                if(Vehicles != null)
                    foreach(GarageWindowVehicle vehicle in Vehicles)
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
