using PARADOX_RP.UI.Models;
using PARADOX_RP.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Async;
using Newtonsoft.Json;
using PARADOX_RP.Core.Factories;

namespace PARADOX_RP.UI
{
    class WindowManager : IWindowManager
    {
        public static WindowManager Instance { get; private set; }

        private readonly Dictionary<Type, object> _windows = new Dictionary<Type, object>();
        public WindowManager(IEnumerable<IWindow> windows)
        {
            Instance = this;

            windows.ForEach(window =>
            {
                _windows[window.GetType()] = window;
                AltAsync.Log($"[{window.WindowName}] Window loaded.");
            });
        }

        public T Get<T>() where T : Window
        {
            if (!Instance._windows.TryGetValue(typeof(T), out var component)) return null;
            return component as T;
        }
    }
}
