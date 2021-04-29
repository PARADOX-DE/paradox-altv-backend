using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Controllers.Team.Interface;
using PARADOX_RP.Core.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Injury
{
    public class InjuryModule : ModuleBase<InjuryModule>
    {
        private IEventController _eventController;
        private ITeamController _teamController;
        public InjuryModule(IEventController eventController, ITeamController teamController) : base("Injury")
        {
            _eventController = eventController;
            _teamController = teamController;
        }

    }
}
