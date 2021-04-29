using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Controllers.Team.Interface;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Injury.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Injury
{
    public enum InjuryTypes
    {
        OTHER,
        KNOCK_OUT,
        CUT,
        SHOT
    }

    public class InjuryModule : ModuleBase<InjuryModule>
    {
        private IEventController _eventController;
        private ITeamController _teamController;

        //TODO: add dbInjury Model
        private readonly Dictionary<uint, object> _injuries;

        public InjuryModule(IEventController eventController, ITeamController teamController) : base("Injury")
        {
            _eventController = eventController;
            _teamController = teamController;

            _injuries = new Dictionary<uint, object>();
        }


        public override async void OnPlayerDeath(PXPlayer player, PXPlayer killer, uint weapon)
        {
            //InjuryModule only for injuries in dimension 0
            if (player.Dimension != 0) return;

            if (_injuries.TryGetValue(weapon, out object injuryType))
            {

            }
            else
            {
                //Injury not found in database 
                await player.Revive();
            }
        }
    }
}
