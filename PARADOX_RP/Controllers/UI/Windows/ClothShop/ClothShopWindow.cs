using AltV.Net;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.UI.Models;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PARADOX_RP.Controllers.UI.Windows.ClothShop
{
    class ClothShopWindow : Window
    {
        public ClothShopWindow() : base("ClothShop") { }
    }

    public class ClothShopWindowWriter : IWritable
    {
        public ClothShopWindowWriter(int component, IEnumerable<Clothes> clothes)
        {
            Component = component;
            Clothes = clothes;
        }

        public int Component { get; set; }
        public IEnumerable<Clothes> Clothes { get; set; }

        public void OnWrite(IMValueWriter writer)
        {
            writer.BeginObject();

            writer.Name("component");
            writer.Value(Component);

            writer.Name("clothes");
            writer.BeginArray();

            foreach (var Cloth in Clothes)
            {
                writer.BeginObject();

                writer.Name("n");
                writer.Value(Cloth.Name);

                writer.Name("v");
                writer.BeginArray();

                foreach (var Variants in Cloth.Variants)
                {
                    writer.BeginObject();

                    writer.Name("c");
                    writer.Value(Variants.Component);

                    writer.Name("d");
                    writer.Value(Variants.Drawable);

                    writer.Name("t");
                    writer.Value(Variants.Texture);

                    writer.Name("t_d");
                    writer.Value(Variants.TorsoDrawable);

                    writer.Name("t_t");
                    writer.Value(Variants.TorsoTexture);

                    writer.Name("n");
                    writer.Value(Variants.Name);

                    writer.EndObject();
                }

                writer.EndArray();

                writer.Name("p");
                writer.Value(Cloth.Price);

                writer.EndObject();
            }

            writer.EndArray();
            writer.EndObject();
        }
    }
}
