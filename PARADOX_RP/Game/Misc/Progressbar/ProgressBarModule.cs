using Newtonsoft.Json;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.UI;
using PARADOX_RP.UI.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Misc.Progressbar
{
    class ProgressBarModule : ModuleBase<ProgressBarModule>
    {
        public ProgressBarModule() : base("ProgressBarModule") { }

        public async void RunProgressBar(PXPlayer player, Action action, int duration)
        {
            WindowManager.Instance.Get<ProgressBarWindow>().Show(player, JsonConvert.SerializeObject(new ProgressBarWindowObject(duration)));
            await Task.Delay(duration);

            player.CheckIfEntityExists();
            if (!player.Exists || !player.LoggedIn) return;

                action();
        }
    }
}
