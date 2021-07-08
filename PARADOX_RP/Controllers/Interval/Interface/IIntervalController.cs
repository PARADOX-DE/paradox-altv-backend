using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace PARADOX_RP.Controllers.Interval.Interface
{
    public interface IIntervalController
    {
        void SetInterval(float duration, ElapsedEventHandler handler);

        Timer GetIntervalByDuration(float duration);
    }
}
