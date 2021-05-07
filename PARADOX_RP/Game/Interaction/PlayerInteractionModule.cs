using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        public override Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key)
        {
            if (key == KeyEnumeration.Y)
            {
                if (player.VoiceRange == VoiceRangeEnumeration.LOW)
                    player.VoiceRange = VoiceRangeEnumeration.MEDIUM;
                else if (player.VoiceRange == VoiceRangeEnumeration.MEDIUM)
                    player.VoiceRange = VoiceRangeEnumeration.HIGH;
                else if (player.VoiceRange == VoiceRangeEnumeration.HIGH)
                {
                    if (player.DutyType != DutyTypes.OFFDUTY)
                        player.VoiceRange = VoiceRangeEnumeration.SPECIAL;
                    else
                        player.VoiceRange = VoiceRangeEnumeration.LOW;
                }
                else
                    player.VoiceRange = VoiceRangeEnumeration.LOW;

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
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
