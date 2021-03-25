using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Handlers.Team;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Team
{

    class TeamModule : ModuleBase<TeamModule>
    {
        public readonly Dictionary<int, Teams> TeamList;
        private readonly TeamHandler _teamHandler;

        public TeamModule(TeamHandler teamHandler) : base("Team") {
            _teamHandler = teamHandler;
        }

        
    }
}
