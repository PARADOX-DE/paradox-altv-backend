using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Utils.Interface
{
    public interface ILogger
    {
        void Console(ConsoleLogType type, string Category, string Log);
    }
}
