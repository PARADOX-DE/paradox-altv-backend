using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.UI.Windows.NativeMenu.Interface
{
    public class NativeMenuItem
    {
        public NativeMenuItem(string text, NativeMenuItemTypes type, bool selected, bool visible = true)
        {
            Text = text;
            Type = type;
            Selected = selected;
            Visible = visible;
        }

        public string Text { get; set; }
        public NativeMenuItemTypes Type { get; set; }
        public bool Selected { get; set; }
        public bool Visible { get; set; }
    }
}
