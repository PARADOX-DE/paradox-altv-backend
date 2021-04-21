using PARADOX_RP.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.UI.Windows
{
    class LoginWindow : Window
    {
        public LoginWindow() : base("Login") { }
    }

    class LoginWindowObject
    {
        public string name { get; set; }
    }
}
