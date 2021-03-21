using AltV.Net.Data;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.MiniGames.Interfaces;
using PARADOX_RP.Game.MiniGames.Models;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.MiniGames.Content.SuperMario
{
    class SuperMarioMinigameModule : ModuleBase<SuperMarioMinigameModule>, IMinigame
    {
        private Position _spawnPoint = new Position(0, 0, 0);
        public Dictionary<int, SuperMarioPickup> _pickups;
        public int _pickupId;

        public SuperMarioMinigameModule() : base("SuperMarioMinigame") {
            _pickupId = 1;
            _pickups = new Dictionary<int, SuperMarioPickup>();
        }

        public MinigameTypes MinigameType { get => MinigameTypes.SUPERMARIO; }

        public void EnteredMinigame(PXPlayer player)
        {
            player.Position = _spawnPoint;
        }

        public override async Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key)
        {
            if(key == KeyEnumeration.E)
            {
                new SuperMarioPickup(SuperMarioPickupTypes.BOMB, player.Position);
            }

            return await Task.FromResult(false);
        }
    }
}
