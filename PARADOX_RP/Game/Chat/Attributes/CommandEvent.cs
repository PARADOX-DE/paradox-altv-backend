using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandEvent : Attribute
    {
        public CommandEventType EventType { get; }

        public CommandEvent(CommandEventType eventType)
        {
            EventType = eventType;
        }
    }
}
