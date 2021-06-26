using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Char.Models
{
    public class OpacityOverlayModel
    {
        public int id { get; set; }
        public int opacity { get; set; }
        public int increment { get; set; }
        public string label { get; set; }
        public int max { get; set; }
        public int min { get; set; }
        public int value { get; set; }
    }
}
