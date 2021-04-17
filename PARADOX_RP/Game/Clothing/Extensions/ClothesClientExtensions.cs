using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Clothing.Extensions
{
    public static class ClothesClientExtensions
    {
        public static void AssignLoadedClothes(this PXPlayer client)
        {
            if (!client.IsValid()) return;
            if (client.Clothes == null) return;

            client.Clothes.ForEach(async (cloth) =>
            {
                if (cloth.Value != null)
                    await client.SetClothes(cloth.Value.Component, cloth.Value.Drawable, cloth.Value.Texture);
            });
        }
    }
}
