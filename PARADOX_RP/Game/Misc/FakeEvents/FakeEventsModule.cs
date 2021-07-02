using AltV.Net.Async;
using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Moderation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Misc.FakeEvents
{
    class FakeEventsModule : ModuleBase<FakeEventsModule>
    {
        private List<string> _fakeEvents = new List<string>()
        {
            "SetMoney"
        };

        public FakeEventsModule(IEventController eventController) : base("FakeEvents") {
            _fakeEvents.ForEach((e) => eventController.OnClient<PXPlayer, object[]>(e, BanPlayer));
        }

        private async void BanPlayer(PXPlayer player, object[] args) => await ModerationModule.Instance.BanPlayer(player, "System", ModuleName);
    }
}
