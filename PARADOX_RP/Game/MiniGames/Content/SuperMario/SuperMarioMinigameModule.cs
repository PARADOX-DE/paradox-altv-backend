using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using EntityStreamer;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Lobby;
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
        private Position _spawnPoint = new Position(3755.6572f, -798.4879f, 47.696655f);

        private string vehicleHash = "blazer5";
        private List<Position> _vehicleSpawns = new List<Position>()
        {
            new Position(3718.576f, -802.37805f, 47.460815f),
            new Position(3715.754f, -794.0044f, 47.44397f),
            new Position(3726.3691f, -793.9121f, 47.47766f),
            new Position(3728.268f, -802.87915f, 47.47766f),
        };


        public Dictionary<int, SuperMarioPickup> _pickups;
        public int _pickupId;

        public SuperMarioMinigameModule() : base("SuperMarioMinigame")
        {
            _pickupId = 1;
            _pickups = new Dictionary<int, SuperMarioPickup>();
        }

        public MinigameTypes MinigameType { get => MinigameTypes.SUPERMARIO; }

        public async void PrepareLobby(LobbyModel model)
        {
            foreach (Position tmpPosition in _vehicleSpawns)
            {
                IVehicle vehicle = await AltAsync.CreateVehicle(Alt.Hash(vehicleHash), tmpPosition, new Rotation(0, 0, 1.6f));
                await vehicle.SetDimensionAsync(model.OwnerId);
                await vehicle.SetPrimaryColorAsync((byte)new Random().Next(0, 70));

                await vehicle.SetEngineOnAsync(false);
            }
        }

        public void EnteredMinigame(PXPlayer player)
        {
            player.Position = _spawnPoint;
        }

        public override async Task<bool> OnColShapeEntered(PXPlayer player, IColShape col)
        {
            if (col.HasData("superMarioPickupId"))
            {
                col.GetData("superMarioPickupId", out int _colShapePickupId);
                if (_colShapePickupId < 0) return await Task.FromResult(false);

                if (_pickups.TryGetValue(_colShapePickupId, out SuperMarioPickup pickup))
                {
                    // if (pickup.LastUsed.Subtract(DateTime.Now).TotalMinutes > 5)
                    //{

                    switch (pickup.PickupType)
                    {
                        case SuperMarioPickupTypes.BOMB:
                            SuperMarioMinigameItemScripts.Instance.pickupSpeed(player);
                            break;
                    };

                    pickup.LastUsed = DateTime.Now;
                    pickup.Object.LightColor = new Rgb(0, 0, 0);
                    await Task.Delay(4500);
                    pickup.Object.LightColor = new Rgb(255, 255, 0);

                    // }
                }

                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        public override async Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key)
        {
            if (key == KeyEnumeration.E)
            {
                if (player.Minigame == MinigameTypes.SUPERMARIO)
                {
                    Position save = player.Position;

                    await Task.Delay(1500);
                    new SuperMarioPickup(SuperMarioPickupTypes.BOMB, save, player.Dimension);
                    return await Task.FromResult(true);
                }
            }

            return await Task.FromResult(false);
        }


    }
}
