using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Core.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Anticheat
{
    class AnticheatModule : ModuleBase<AnticheatModule>
    {
        public AnticheatModule(IEventController eventController) : base("Anticheat")
        {
            
        }
    }
}
