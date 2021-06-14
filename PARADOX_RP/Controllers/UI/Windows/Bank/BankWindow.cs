using AltV.Net;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.UI.Windows
{
    class BankWindow : Window
    {
        public BankWindow() : base("Bank") { }
    }

    class BankWindowWriter : IWritable
    {
        private string PlayerName;
        private int Wallet;
        private int Bank;
        private List<PlayerBankHistory> History;

        public BankWindowWriter(string playerName, int wallet, int bank, List<PlayerBankHistory> history)
        {
            PlayerName = playerName;
            Wallet = wallet;
            Bank = bank;
            History = history;
        }

        public void OnWrite(IMValueWriter writer)
        {
            writer.BeginObject();

            writer.Name("PlayerName");
            writer.Value(PlayerName);

            writer.Name("Wallet");
            writer.Value(Wallet);

            writer.Name("Bank");
            writer.Value(Bank);


            writer.Name("History");
            writer.BeginArray();

            foreach (PlayerBankHistory historyItem in History)
            {
                writer.BeginObject();

                writer.Name("name");
                writer.Value(historyItem.Name);

                writer.Name("date");
                writer.Value(historyItem.Date.ToString("yyyy-MM-ddTHH:mm:ss"));

                writer.Name("act");
                writer.Value((int)historyItem.Action);

                writer.Name("money");
                writer.Value(historyItem.Money);

                writer.EndObject();
            }

            writer.EndArray();

            writer.EndObject();
        }
    }
}
