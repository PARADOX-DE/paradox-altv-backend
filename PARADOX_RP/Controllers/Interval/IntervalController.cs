using PARADOX_RP.Controllers.Interval.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace PARADOX_RP.Controllers.Interval
{
    public class IntervalController : IIntervalController
    {
        private readonly Dictionary<float, Timer> _intervals = new Dictionary<float, Timer>();

        public void SetInterval(float duration, ElapsedEventHandler handler)
        {
            var timer = new Timer(duration);
            timer.Elapsed += handler;
            timer.Start();

            _intervals.Add(duration, timer);
        }

        public Timer GetIntervalByDuration(float duration)
        {
            if (_intervals.TryGetValue(duration, out Timer interval))
                return interval;

            return null;
        }
    }
}
