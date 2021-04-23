using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Controllers.Event.Interface
{
    interface IEventController
    {
        void EventReceived(PXPlayer player, string eventName, params object[] args);
    }
}
