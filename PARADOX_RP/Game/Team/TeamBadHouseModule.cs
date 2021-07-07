using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Core.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Team
{
    public sealed class TeamBadHouseModule : Module<TeamBadHouseModule>
    {
        private readonly IEventController _eventController;
        private readonly I _eventController;

        public TeamBadHouseModule() : base("TeamBadHouse")
        {

        }
    }
}
