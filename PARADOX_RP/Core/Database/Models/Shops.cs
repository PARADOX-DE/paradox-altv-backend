using AltV.Net.Data;
using PARADOX_RP.Game.Inventory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    public partial class ShopItem
    {
        public ShopItem(int id, string name, int price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        public int Id { get; set; }
        public string Name { get; set; }
<<<<<<< HEAD
        public int Price { get; set; }
    }

    public partial class Shops
    {
        public int Id { get; set; }
        public List<ShopItem> Items { get; set; }
=======
        public int TeamId { get; set; }
        public virtual Teams Team { get; set; }
        public virtual ICollection<ShopItems> Items { get; set; }
>>>>>>> b24d8a5a29ed443c37c053bac3914fe8a6a94b53

        public float Position_X { get; set; }
        public float Position_Y { get; set; }
        public float Position_Z { get; set; }
        public float Rotation_X { get; set; }
        public float Rotation_Y { get; set; }
        public float Rotation_Z { get; set; }
    }

    public partial class Shops
    {
        public Position Position => new Position(Position_X, Position_Y, Position_Z);
        public Rotation Rotation => new Rotation(Rotation_X, Rotation_Y, Rotation_Z);
    }
}
