using AltV.Net;
using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Controllers.Event
{
    class EventController : IEventController
    {
        public static EventController Instance { get; } = new EventController();

        private List<string> _whitelistedEvents = new List<string>();

        public EventController() { }

        [ScriptEvent(ScriptEventType.PlayerEvent)]
        public void EventReceived(PXPlayer player, string eventName, params object[] args)
        {
            //TODO: event receiving lol
        }

        public void WhitelistEvent(string eventName)
        {
            _whitelistedEvents.Add(eventName);
        }
    }
}
