using AltV.Net.Async;
using Newtonsoft.Json;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.UI.Models
{
    class Window : IWindow
    {
        public bool Enabled { get; set; }
        public string WindowName { get; set; }

        public Window(string WindowName, bool Enabled = true)
        {
            this.WindowName = WindowName;
            this.Enabled = Enabled;
        }

        public void Show(PXPlayer player, params object[] windowObject)
        {
            if (Configuration.Instance.DevMode)
            {
                AltAsync.Log($"[{WindowName}] Show {player.Name}");
            }

            player.CurrentWindow = WindowName;
            player.Emit("Webview::ShowWindow", WindowName, windowObject);
        }

        public void Hide(PXPlayer player)
        {
            player.CurrentWindow = "";
            player.Emit("Webview::CloseWindow", WindowName);
        }

        public bool IsVisible(PXPlayer player)
        {
            if (player.CurrentWindow == WindowName)
                return true;

            return false;
        }
    }
}
