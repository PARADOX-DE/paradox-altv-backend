using AltV.Net.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    public partial class Eastereggs
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PedHash { get; set; }
        public string AnimationDictionary { get; set; }
        public string AnimationName { get; set; }
        public float Position_X { get; set; }
        public float Position_Y { get; set; }
        public float Position_Z { get; set; }
        public int Duration { get; set; }
        public int Money { get; set; }
    }

    public partial class Eastereggs
    {
        public Position Position => new Position(Position_X, Position_Y, Position_Z);
    }
}
