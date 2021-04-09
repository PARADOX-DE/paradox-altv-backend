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

    class BankWindowObject
    {
        public BankWindowObject(string playerName)
        {
            PlayerName = playerName;
        }

        public string PlayerName { get; set; }
        
        /*
         *
         * TODO: Add BankHistory
         *
         */
    }
}
