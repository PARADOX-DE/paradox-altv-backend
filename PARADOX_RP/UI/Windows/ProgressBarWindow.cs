using PARADOX_RP.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.UI.Windows
{
    class ProgressBarWindow : Window
    {
        public ProgressBarWindow() : base("ProgressBar") { }
    }

    class ProgressBarWindowObject
    {
        public ProgressBarWindowObject(int duration)
        {
            this.duration = duration;
        }

        public int duration { get; set; }
    }
}
