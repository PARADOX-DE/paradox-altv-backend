using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Administration.Models
{
    public class SupportRankModel
    {
        public SupportRankModel(int id, string name, int color_R, int color_G, int color_B)
        {
            Id = id;
            Name = name;
            Color_R = color_R;
            Color_G = color_G;
            Color_B = color_B;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Color_R { get; set; }
        public int Color_G { get; set; }
        public int Color_B { get; set; }
    }
}
