using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Misc.Progressbar.Extensions
{
    static class ProgressBarClientExtensions
    {
        public static async Task<bool> RunProgressBar(this PXPlayer player, Func<Task> action, string title, string message, int duration, bool abortable = true) => await ProgressBarModule.Instance.RunProgressBar(player, action, title, message, duration, abortable);
        public static bool CancelProgressBar(this PXPlayer player) => ProgressBarModule.Instance.CancelProgressBar(player);
    }
}
