using AltV.Net;
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

    class ConfirmationWindowWriter : IWritable
    {
        public ConfirmationWindowWriter(string title, string description, string acceptCallback, string declineCallback)
        {
            Title = title;
            Description = description;
            AcceptCallback = acceptCallback;
            DeclineCallback = declineCallback;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string AcceptCallback { get; set; }
        public string DeclineCallback { get; set; }

        public void OnWrite(IMValueWriter writer)
        {
            writer.BeginObject();
            writer.Name("Title");
            writer.Value(Title);
            writer.Name("Description");
            writer.Value(Description);
            writer.Name("AcceptCallback");
            writer.Value(AcceptCallback);
            writer.Name("DeclineCallback");
            writer.Value(DeclineCallback);
            writer.EndObject();
        }
    }
}
