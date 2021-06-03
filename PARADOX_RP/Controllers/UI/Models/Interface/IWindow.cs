using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.UI.Models
{
    interface IWindow
    {
        bool Enabled { get; set; }
        string WindowName { get; set; }
    }
}
