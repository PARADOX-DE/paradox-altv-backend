using PARADOX_RP.Controllers.Interval.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace PARADOX_RP.Controllers.Interval
{
    class IntervalController : IIntervalController
    {
        public void SetInterval(float duration, ElapsedEventHandler handler)
        {
            var timer = new Timer(duration);
            timer.Elapsed += handler;
            timer.Start();
        }
    }
}
