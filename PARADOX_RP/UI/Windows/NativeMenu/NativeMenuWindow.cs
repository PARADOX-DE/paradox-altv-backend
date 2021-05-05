using AltV.Net;
using AltV.Net.Async;
using Newtonsoft.Json;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.UI.Windows.NativeMenu
{
    class NativeMenuWindow : Window
    {
        private readonly Dictionary<Type, INativeMenu> _nativeMenus = new Dictionary<Type, INativeMenu>();

        public NativeMenuWindow(IEnumerable<INativeMenu> nativeMenus) : base("NativeMenu") {
            nativeMenus.ForEach(menu =>
            {
                _nativeMenus[menu.GetType()] = menu;
                AltAsync.Log($"[NativeMenu] {menu.Title} Menu loaded.");
            });
        }


        public void DisplayMenu<T>(PXPlayer player) where T : INativeMenu {
            if (!_nativeMenus.TryGetValue(typeof(T), out var component)) return;

            AltAsync.Log(component.Title);
        }
    }
}
