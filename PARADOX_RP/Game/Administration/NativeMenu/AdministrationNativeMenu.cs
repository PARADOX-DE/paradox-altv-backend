using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.UI.Windows.NativeMenu;
using PARADOX_RP.UI.Windows.NativeMenu.Interface;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Administration.NativeMenu
{
    public class AdministrationNativeMenu : INativeMenu
    {
        public string Title => "Administration";

        public string Description => "Serververwaltung";

        public List<NativeMenuItem> Items => new List<NativeMenuItem>()
        {
            new NativeMenuItem("Schliessen", NativeMenuItemTypes.Button, true),
            new NativeMenuItem("Verwaltung: Spieler", NativeMenuItemTypes.Button, false),
        };

        public void Callback(PXPlayer player, NativeMenuItem item)
        {

        }
    }
}
