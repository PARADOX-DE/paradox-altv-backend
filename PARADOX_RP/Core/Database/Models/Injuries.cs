using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    public class Injuries
    {
        public int Id { get; set; }
        public uint Weapon { get; set; }
        public string Name { get; set; }
        public string EffectName { get; set; }
        public string Sound { get; set; }
    }
}
