using AltV.Net.Data;
using PARADOX_RP.Game.Inventory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    public partial class Shops
    {
        public Shops()
        {
            Items = new HashSet<ShopItems>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int TeamId { get; set; }
        public virtual ICollection<ShopItems> Items { get; set; }

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
