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
            ColoredMessage message = new ColoredMessage();
            message += "[";
            message += type == ConsoleLogType.SUCCESS ? TextColor.Green : TextColor.Red;
            message += type == ConsoleLogType.SUCCESS ? "+" : "x";
            message += "] ";

            message += $"{Category} >> {Log}";

            Alt.LogColored(message);
        }
    }
}
