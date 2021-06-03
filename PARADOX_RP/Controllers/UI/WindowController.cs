using PARADOX_RP.UI.Models;
using PARADOX_RP.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Async;
using Newtonsoft.Json;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Utils.Interface;
using PARADOX_RP.Utils;

namespace PARADOX_RP.UI
{
    class WindowController : IWindowController
    {
        public static WindowController Instance { get; private set; }

        private readonly Dictionary<Type, object> _windows = new Dictionary<Type, object>();
        public WindowController(IEnumerable<IWindow> windows, ILogger logger)
        {
            Instance = this;

            windows.ForEach(window =>
            {
                _windows[window.GetType()] = window;
            });

            logger.Console(ConsoleLogType.SUCCESS, "Initializing", "Successfully initialized UI Windows!");
        }

        public T Get<T>() where T : Window
        {
            if (!Instance._windows.TryGetValue(typeof(T), out var component)) return null;
            return component as T;
        }
    }
}
