using AltV.Net;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.UI.Windows
{
    class GasStationWindow : Window
    {
        public GasStationWindow() : base("GasStation") { }
    }

    class GasStationWindowWriter : IWritable
    {
        public GasStationWindowWriter(int id, string gasstationName, int playerMoney, int petrol, int diesel, int electro)
        {
            Id = id;
            GasStationName = gasstationName;
            PlayerMoney = playerMoney;
            Petrol = petrol;
            Diesel = diesel;
            Electro = electro;
        }

        private int Id { get; set; }
        private string GasStationName { get; set; }
        private int PlayerMoney { get; set; }
        private int Petrol { get; set; }
        private int Diesel { get; set; }
        private int Electro { get; set; }

        public void OnWrite(IMValueWriter writer)
        {
            writer.BeginObject();
            writer.Name("id");
            writer.Value(Id);

            writer.Name("name");
            writer.Value(GasStationName);

            writer.Name("player_money");
            writer.Value(PlayerMoney);

            writer.Name("petrol");
            writer.Value(Petrol);

            writer.Name("diesel");
            writer.Value(Diesel);

            writer.Name("electro");
            writer.Value(Electro);

            writer.EndObject();
        }
    }
}
