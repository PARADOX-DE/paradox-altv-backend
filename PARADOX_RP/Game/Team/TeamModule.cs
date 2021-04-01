using Microsoft.EntityFrameworkCore;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Handlers.Team;
using PARADOX_RP.Handlers.Team.Interface;
using PARADOX_RP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PARADOX_RP.Game.Team
{

    class TeamModule : ModuleBase<TeamModule>
    {
        public Dictionary<int, Teams> TeamList;
        private readonly ITeamHandler _teamHandler;

        public TeamModule(ITeamHandler teamHandler) : base("Team")
        {
            TeamList = new Dictionary<int, Teams>();

            using (var px = new PXContext())
            {
                foreach (Teams team in px.Teams)
                {
                    TeamList.Add(team.Id, team);
                    _teamHandler.LoadTeam(team);
                }
            }

            _teamHandler = teamHandler;
        }

        public void RequestTeamMembers(PXPlayer player, bool onlineState)
        {
            IEnumerable<PXPlayer> _playerPool = (IEnumerable<PXPlayer>)Pools.Instance.Get(PoolType.PLAYER);
            IEnumerable<PXPlayer> _factionMembers = null;
            if (onlineState) _factionMembers = _playerPool.Where(p => p.Team.Id == player.Team.Id);


        }
    }
}
