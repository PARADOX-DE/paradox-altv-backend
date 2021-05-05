using PARADOX_RP.Core.Factories;
using PARADOX_RP.UI.Windows.NativeMenu.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.UI.Windows.NativeMenu
{
    public interface INativeMenu
    {
        string Title { get; }
        string Description { get; }
        List<NativeMenuItem> Items { get; }

        void Callback(PXPlayer player, NativeMenuItem item);
    }
}
