using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Game.Lobby;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    [Table("paintball_maps")]
    public partial class PaintballMaps
    {
        public PaintballMaps()
        {
            Flags = new HashSet<PaintballMapsFlags>();
            Spawns = new HashSet<PaintballMapsSpawns>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public float Position_X { get; set; }
        public float Position_Y { get; set; }
        public float Position_Z { get; set; }

        public float Radius { get; set; }

        public float PreviewPosition_X { get; set; }
        public float PreviewPosition_Y { get; set; }
        public float PreviewPosition_Z { get; set; }

        public float QueuePosition_X { get; set; }
        public float QueuePosition_Y { get; set; }
        public float QueuePosition_Z { get; set; }

        public virtual ICollection<PaintballMapsFlags> Flags { get; set; }
        public virtual ICollection<PaintballMapsSpawns> Spawns { get; set; }
    }

    public partial class PaintballMaps
    {
        public Position Position => new Position(Position_X, Position_Y, Position_Z);
        public Position PreviewPosition => new Position(PreviewPosition_X, PreviewPosition_Y, PreviewPosition_Z);
        public Position QueuePosition => new Position(QueuePosition_X, QueuePosition_Y, QueuePosition_Z);
    }
}
