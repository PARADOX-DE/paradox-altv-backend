using AltV.Net;
using AltV.Net.Async;
using AltV.Net.ColoredConsole;
using PARADOX_RP.Utils.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Utils
{
    public enum ConsoleLogType
    {
        ERROR,
        SUCCESS
    }

    public class Logger : ILogger
    {
        public void Console(ConsoleLogType type, string Category, string Log)
        {
            string message = "[";
            message += type == ConsoleLogType.SUCCESS ? "~g~" : "~r~";
            message += type == ConsoleLogType.SUCCESS ? "+" : "x";
            message += "~w~";
            message += "] ";

            message += $"{Category} >> {Log}";

            ColoredMessage cMessage = new ColoredMessage();
            cMessage += message;

            Alt.LogColored(cMessage);
        }
    }
}
