using AltV.Net.Async;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using PARADOX_RP.Core.Database;
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
            using (var px = new PXContext())
            {
                var databaseCreator = (px.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator);
                databaseCreator.CreateTables();

                AltAsync.Log("Initialized Database.");
            }

            AltAsync.Log("Server started.");
            _eventHandler.Load();
        }
    }
}
