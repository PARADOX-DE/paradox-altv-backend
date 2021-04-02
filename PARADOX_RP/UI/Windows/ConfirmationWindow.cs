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
        public ConfirmationWindowObject(string title, string message, string acceptCallback, string declineCallback)
        {
            Title = title;
            Message = message;
            AcceptCallback = acceptCallback;
            DeclineCallback = declineCallback;
        }

        public string Title { get; set; }
        public string Message { get; set; }
        public string AcceptCallback { get; set; }
        public string DeclineCallback { get; set; }
    }
}
