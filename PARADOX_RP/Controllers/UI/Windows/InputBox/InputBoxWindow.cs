using AltV.Net;
using PARADOX_RP.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.UI.Windows
{
    class InputBoxWindow : Window
    {
        public InputBoxWindow() : base("Confirmation") { }
    }

    class InputBoxWindowWriter : IWritable
    {
        public InputBoxWindowWriter(string title, string description, string placeholder, string acceptText, string acceptCallback)
        {
            Title = title;
            Description = description;
            Placeholder = placeholder;
            AcceptText = acceptText;
            AcceptCallback = acceptCallback;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Placeholder { get; set; }
        public string AcceptText { get; set; }
        public string AcceptCallback { get; set; }

        public void OnWrite(IMValueWriter writer)
        {
            writer.BeginObject();
            writer.Name("Title");
            writer.Value(Title);
            writer.Name("Description");
            writer.Value(Description);
            writer.Name("Placeholder");
            writer.Value(Placeholder);
            writer.Name("AcceptText");
            writer.Value(AcceptText);
            writer.Name("AcceptCallback");
            writer.Value(AcceptCallback);
            writer.EndObject();
        }
    }
}
