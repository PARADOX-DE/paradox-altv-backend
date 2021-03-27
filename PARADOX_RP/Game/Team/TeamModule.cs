using Microsoft.EntityFrameworkCore;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Handlers.Team;
using PARADOX_RP.Handlers.Team.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Team
{

    class TeamModule : ModuleBase<TeamModule>
    {
        public Dictionary<int, Teams> TeamList;
        private readonly ITeamHandler _teamHandler;

        public TeamModule(ITeamHandler teamHandler) : base("Team") {
            TeamList = new Dictionary<int, Teams>();

            _teamHandler = teamHandler;
        }
    }
}
