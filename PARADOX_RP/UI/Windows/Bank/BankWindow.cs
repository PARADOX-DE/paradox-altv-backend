using AltV.Net;
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

        public BankWindowWriter(string playerName, int wallet, int bank)
        {
            PlayerName = playerName;
            Wallet = wallet;
            Bank = bank;
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
            writer.EndObject();
        }
    }
}
