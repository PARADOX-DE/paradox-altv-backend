using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Handlers.Team;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Team
{

    class TeamModule : ModuleBase<TeamModule>
    {
        public Dictionary<int, Teams> TeamList;
        private readonly TeamHandler _teamHandler;

        public TeamModule(TeamHandler teamHandler) : base("Team") {
            TeamList = new Dictionary<int, Teams>();

            _teamHandler = teamHandler;
        }

        public override void OnModuleLoad()
        {
            using(var px = new PXContext())
            {
                px.Teams.ForEach((team) =>
                {
                    TeamList.Add(team.Id, team);
                });
            }
        }
    }
}
