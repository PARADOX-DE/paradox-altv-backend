using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Team
{

    class TeamModule : ModuleBase<TeamModule>
    {
        public readonly Dictionary<int, Teams> TeamList;

        public TeamModule() : base("Team") { }

        
    }
}
