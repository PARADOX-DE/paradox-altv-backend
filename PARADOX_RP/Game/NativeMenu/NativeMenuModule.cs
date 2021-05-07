using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.UI.Windows.NativeMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PARADOX_RP.Game.NativeMenu
{
    public class NativeMenuModule : ModuleBase<NativeMenuModule>
    {
        private IEnumerable<INativeMenu> _nativeMenus;
        private IEventController _eventController;

        public NativeMenuModule(IEnumerable<INativeMenu> nativeMenus, IEventController eventController) : base("NativeMenu")
        {
            _nativeMenus = nativeMenus;
            _eventController = eventController;

            _eventController.OnClient<PXPlayer, string, string>("NativeMenuCallback", NativeMenuCallback);
        }

        private void NativeMenuCallback(PXPlayer player, string ItemName, string InputString = "")
        {
            if (!player.IsValid()) return;
            
            INativeMenu menu = player.CurrentNativeMenu;
            if (menu == null) return;

            var item = menu.Items.FirstOrDefault((i) => i.Text == ItemName);
            if (item == null) return;

            menu.Callback(player, item, InputString);
        }
    }
}
