using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Core.Events.Intervals
{
    public interface IEventIntervalMinute
    {
        Task OnEveryMinute();
    }
}
