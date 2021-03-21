using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using DasNiels.AltV.Streamers;
using PARADOX_RP.Game.MiniGames.Content.SuperMario.Models;
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

        public SuperMarioPickup(Position Position, SuperMarioPickupTypes pickupType)
        {
            ///DynamicObject obj = ObjectStreamer.CreateDynamicObject("OBJ", position, position, visible: true, streamRange: 100, frozen: true);
            this.Position = Position;
            this.Position = Position;

        }

        public SuperMarioPickup(SuperMarioPickupTypes pickupType, Position position)
        {
            IColShape colShape = Alt.CreateColShapeCylinder(position, 2, 2);

            Id = SuperMarioMinigameModule.Instance._pickupId;
            PickupType = pickupType;
            Position = position;
            LastUsed = DateTime.Now;

            SuperMarioMinigameModule.Instance._pickups.Add(Id, this);
            SuperMarioMinigameModule.Instance._pickupId++;
        }
    }
}
