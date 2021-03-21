using AltV.Net.Async;
using AltV.Net.EntitySync;
using AltV.Net.EntitySync.ServerEvent;
using AltV.Net.EntitySync.SpatialPartitions;
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
                databaseCreator.EnsureCreated();

                AltAsync.Log("Initialized Database.");
            }

            AltEntitySync.Init(1, 100,
                (threadCount, repository) => new ServerEventNetworkLayer(threadCount, repository),
                (entity, threadCount) => entity.Type,
                (entityId, entityType, threadCount) => entityType,
                (threadId) => new LimitedGrid3(50_000, 50_000, 100, 10_000, 10_000, 600),
                new IdProvider()
            );

            AltAsync.Log("Server started.");
            _eventHandler.Load();
        }
    }
}
