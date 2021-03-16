using AltV.Net.Async;
using PARADOX_RP.Core.Interface;
using PARADOX_RP.Handlers;
using PARADOX_RP.UI;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core
{
    class CoreSystem : ICoreSystem
    {
        private readonly IEventHandler _eventHandler;
        private readonly IWindowManager _componentManager;
        public CoreSystem(IEventHandler eventHandler, IWindowManager componentManager)
        {
            _eventHandler = eventHandler;
            _componentManager = componentManager;
        }

        public void Start()
        {
            AltAsync.Log("Server started.");
            _eventHandler.Load();
        }
    }
}
