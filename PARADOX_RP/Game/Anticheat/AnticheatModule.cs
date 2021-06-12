using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Core.Factories;
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
            eventController.OnClient<PXPlayer>("LoadAnticheat", LoadAnticheat);
            eventController.OnClient<PXPlayer>("FlagAnticheat", FlagAnticheat);
        }

        private void FlagAnticheat(PXPlayer player)
        {
            player.SendNotification("Anticheat", "Cheat Injection detected.", NotificationTypes.SUCCESS);
        }

        private void LoadAnticheat(PXPlayer player)
        {
            player.SendNotification("Anticheat", "Loaded successful.", NotificationTypes.SUCCESS);
        }
    }
}
