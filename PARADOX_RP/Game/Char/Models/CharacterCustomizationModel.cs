using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Char.Models
{
    public class CharacterCustomizationModel
    {
        public int Father { get; set; }
        public int Mother { get; set; }
        public double Resemblance { get; set; }
        public int SkinTone { get; set; }
        public List<double> FaceData { get; set; }
        public List<OpacityOverlayModel> OpacityOverlays { get; set; }
        public HairOverlayModel HairOverlay { get; set; }
        public int HairStyle { get; set; }
        public int HairColor { get; set; }
        public int EyebrowShape { get; set; }
        public int EyebrowThickness { get; set; }
        public int EyeColor { get; set; }
        public int Gender { get; set; }
    }
}
