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

namespace PARADOX_RP.Game.MiniGames
{
    class MinigameModule : ModuleBase<MinigameModule>
    {
        private readonly IEnumerable<IMinigame> _minigames;
        public MinigameModule(IEnumerable<IMinigame> minigames) : base("Minigame") {
            _minigames = minigames;
        }

        public void ChooseMinigame(PXPlayer player, MinigameTypes minigame)
        {
            if(player.Minigame != MinigameTypes.NONE)
            {
                player.SendNotification("Minigame", "Du bist bereits in einem Minigame.", NotificationTypes.ERROR);
                return;
            }

            LobbyModel lobby = new LobbyModel()
            {
                Id = 0,
                Owner = player.Username,
                MaxCounts = 12
            };

            player.Minigame = minigame;
            player.Dimension = LobbyModule.Instance.GetDimensionByLobby(lobby);
        }
    }
}
