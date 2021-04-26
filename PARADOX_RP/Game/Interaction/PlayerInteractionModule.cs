using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Interaction
{
    class PlayerInteractionModule : ModuleBase<PlayerInteractionModule>
    {
        private IEventController _eventController;
    
        public PlayerInteractionModule(IEventController eventController) : base("PlayerInteraction")
        {
            _eventController = eventController;

            _eventController.OnClient<PXPlayer, PXPlayer>("CuffPlayer", CuffPlayer);
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
