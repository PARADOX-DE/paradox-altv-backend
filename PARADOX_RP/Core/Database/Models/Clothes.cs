using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    public class Clothes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Component { get; set; }
        public int TorsoComponent { get; set; }
        public int TorsoDrawable { get; set; }
        public int TorsoTexture { get; set; }
        public int Drawable { get; set; }
        public int Texture { get; set; }
        public int Gender { get; set; }
    }
}
