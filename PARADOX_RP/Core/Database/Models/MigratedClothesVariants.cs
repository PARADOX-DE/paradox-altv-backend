using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    public class MigratedClothesVariants
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Component { get; set; }
        public int Drawable { get; set; }
        public int Texture { get; set; }

        public int TorsoComponent { get; set; }
        public int TorsoDrawable { get; set; }
        public int TorsoTexture { get; set; }

        public Gender Gender { get; set; }

        public int ClothId { get; set; }
        public virtual MigratedClothes Cloth { get; set; }
    }
}
