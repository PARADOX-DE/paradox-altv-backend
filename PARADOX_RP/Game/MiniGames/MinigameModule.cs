using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Lobby;
using PARADOX_RP.Game.MiniGames.Interfaces;
using PARADOX_RP.Game.MiniGames.Models;
using System.Collections.Generic;
using System.Linq;

namespace PARADOX_RP.Game.MiniGames
{
    class MinigameModule : ModuleBase<MinigameModule>
    {
        private readonly IEnumerable<IMinigame> _minigames;
        public MinigameModule(IEnumerable<IMinigame> minigames) : base("Minigame")
        {
            _minigames = minigames;
        }

        public void ChooseMinigame(PXPlayer player, MinigameTypes minigame)
        {
            if (player.Minigame != MinigameTypes.NONE)
            {
                player.SendNotification("Minigame", "Du bist bereits in einem Minigame.", NotificationTypes.ERROR);
                return;
            }

            IMinigame minigameInterface = _minigames.FirstOrDefault(i => i.MinigameType == minigame);
            if (minigameInterface == null) return;

            LobbyModel lobby = LobbyModule.Instance.RegisterLobby(player, 12);
            if (lobby == null)
            {
                player.SendNotification("Minigame", "Du hast bereits eine Lobby offen.", NotificationTypes.ERROR);
                return;
            }

            minigameInterface.PrepareLobby(lobby);

            player.Minigame = minigame;
            player.Dimension = LobbyModule.Instance.GetDimensionByLobby(lobby);

            minigameInterface.EnteredMinigame(player);
        }
    }
}
