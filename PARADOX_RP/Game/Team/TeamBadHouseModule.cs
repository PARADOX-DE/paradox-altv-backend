using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Controllers.Team.Interface;
using PARADOX_RP.Core.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Team
{
    public sealed class TeamBadHouseModule : Module<TeamBadHouseModule>
    {
        private readonly IEventController _eventController;
        private readonly ITeamController _teamController;

        public TeamBadHouseModule(IEventController eventController, ITeamController teamController) : base("TeamBadHouse")
        {
            _eventController = eventController;
            _teamController = teamController;
        }
    }
}
