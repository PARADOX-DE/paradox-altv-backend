using AltV.Net.Async;
using AltV.Net.Async.Elements.Refs;
using Newtonsoft.Json;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.UI;
using PARADOX_RP.UI.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Misc.Progressbar
{
    class ProgressBarModule : ModuleBase<ProgressBarModule>
    {
        public ProgressBarModule() : base("ProgressBarModule") { }

        public async Task<bool> RunProgressBar(PXPlayer player, Func<Task> action, string title, string message, int duration)
        {
            // WindowManager.Instance.Get<ProgressBarWindow>().Show(player, JsonConvert.SerializeObject(new ProgressBarWindowObject(duration)));

            using var playerRef = new AsyncPlayerRef(player);
            if (!playerRef.Exists) return false;

            await player.EmitAsync("Progress::Start", title, message, duration);

            player.CancellationToken = new CancellationTokenSource();
            bool result = await Task.Delay(duration, player.CancellationToken.Token).ContinueWith(task => !task.IsCanceled);

            if (result && player != null && await player.ExistsAsync()) await action();

            return result;
        }

        public bool CancelProgressBar(PXPlayer player)
        {
            if (player.CancellationToken != null)
            {
                player.CancellationToken.Cancel();
                player.CancellationToken = null;
                return true;
            }

            return false;
        }
    }
}
