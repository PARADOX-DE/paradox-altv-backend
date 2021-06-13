using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    public class Items
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageName { get; set; }
        public int Weight { get; set; }
        public int StackSize { get; set; }
        public string ScriptName { get; set; }
    }
}
