using AltV.Net;
using PARADOX_RP.Game.Clothing.Models;
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
        public ClothShopWindowWriter(ICollection<IGrouping<ComponentVariation, ShopClothModel>> clothes)
        {
            Clothes = clothes;
        }

        public ICollection<IGrouping<ComponentVariation, ShopClothModel>> Clothes { get; set; }

        public void OnWrite(IMValueWriter writer)
        {
            writer.BeginArray();

            foreach (var Group in Clothes)
            {
                writer.BeginObject();
                    writer.Name(Enum.GetName(typeof(ComponentVariation), Group.Key));
                    writer.BeginArray();
                        foreach(var Cloth in Group) { 
                            writer.BeginObject();

                            writer.Name("name");
                            writer.Value(Cloth.Name);

                            writer.Name("variants");
                            writer.BeginArray();
                
                                foreach (var Variants in Cloth.Variants)
                                writer.Value(Variants.Value.Name);

                            writer.EndArray();

                            writer.Name("price");
                            writer.Value(Cloth.Price);

                            writer.EndObject();
                        }
                    writer.EndArray();

                writer.EndObject();
            }

            writer.EndArray();
        }
    }
}
