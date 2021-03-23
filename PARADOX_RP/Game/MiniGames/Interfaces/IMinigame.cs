using PARADOX_RP.Core.Factories;
using PARADOX_RP.Game.Lobby;
using PARADOX_RP.Game.MiniGames.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.MiniGames.Interfaces
{
    interface IMinigame
    {
        MinigameTypes MinigameType { get; }
        void PrepareLobby(LobbyModel model);
        void EnteredMinigame(PXPlayer player);
    }
}
