using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Misc.Progressbar.Extensions
{
    static class ProgressBarClientExtensions
    {
        public static void RunProgressBar(this PXPlayer player, Action action, int duration) => ProgressBarModule.Instance.RunProgressBar(player, action, duration);
    }
}
