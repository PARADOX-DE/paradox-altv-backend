using PARADOX_RP.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.UI.Windows
{
    class ConfirmationWindow : Window
    {
        public ConfirmationWindow() : base("Confirmation") { }
    }

    class ConfirmationWindowObject
    {
        public string AcceptCallback { get; set; }
        public string DeclineCallback { get; set; }
    }
}
