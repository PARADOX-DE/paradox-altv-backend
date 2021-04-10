using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Interaction
{
    class PlayerInteractionModule : ModuleBase<PlayerInteractionModule>
    {
        public PlayerInteractionModule() : base("PlayerInteraction")
        {

            
        }

        public void CuffPlayer(PXPlayer player, PXPlayer victim)
        {
            if (!player.CanInteract() || !player.IsValid()) return;
            if (!victim.IsValid() || victim.Injured) return;

            if (player.Position.Distance(victim.Position) > 3.5f) return;

            victim.Cuffed = true;
        } 
    }
}
