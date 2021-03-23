using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.MiniGames.Content.SuperMario
{
    class SuperMarioMinigameItemScripts
    {
        public void pickupSpeed(PXPlayer player)
        {
            player.Armor = player.MaxArmor;
        }
    }
}
