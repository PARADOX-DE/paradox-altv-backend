using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using DasNiels.AltV.Streamers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.MiniGames.Content.SuperMario
{
    public enum SuperMarioPickupTypes
    {
        SHELL,
        BANANA,
        MUSHROOM,
        BOMB,
        SPEED
    }

    class SuperMarioPickup
    {
        public int Id { get; set; }
        public IColShape ColShape { get; set; }
        public SuperMarioPickupTypes PickupType { get; set; }
        public Position Position { get; set; }
        public DateTime LastUsed { get; set; }

        public SuperMarioPickup(SuperMarioPickupTypes pickupType, Position position, int dimension)
        {
            Id = SuperMarioMinigameModule.Instance._pickupId;
            PickupType = pickupType;
            Position = position;
            LastUsed = DateTime.Now;

            ColShape = Alt.CreateColShapeCylinder(position, 3, 3);
            ColShape.SetData("superMarioPickupId", Id);
            ColShape.Dimension = dimension;

            Alt.CreateCheckpoint(CheckpointType.Cyclinder, position, 2, 2, new Rgba(255, 0, 0, 255));

            SuperMarioMinigameModule.Instance._pickups.Add(Id, this);
            SuperMarioMinigameModule.Instance._pickupId++;
        }
    }
}
