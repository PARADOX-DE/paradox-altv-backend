using AltV.Net.Async;
using AltV.Net.EntitySync;
using AltV.Net.EntitySync.ServerEvent;
using AltV.Net.EntitySync.SpatialPartitions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Interface;
using PARADOX_RP.Controllers;
using PARADOX_RP.UI;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core
{
    class Application : IApplication
    {
        public string Name { get; } = "PARADOX Roleplay";
        public string Author { get; } = "PARADOX International";

        private readonly IModuleController _moduleController;
        private readonly IWindowManager _componentManager;
        public Application(IModuleController moduleController, IWindowManager componentManager)
        {
            _moduleController = moduleController;
            _componentManager = componentManager;
        }

        public void Start()
        {
            AltEntitySync.Init(3, 250,
                (threadCount, repository) => new ServerEventNetworkLayer(threadCount, repository),
                (entity, threadCount) => entity.Type,
                (entityId, entityType, threadCount) => entityType,
                (threadId) =>
                {
                    if (threadId == 1 || threadId == 0)
                    {
                        return new LimitedGrid3(50_000, 50_000, 75, 10_000, 10_000, 350);
                    }
                    else if (threadId == 2)
                    {
                        return new LimitedGrid3(50_000, 50_000, 125, 10_000, 10_000, 1000);
                    }
                    else
                    {
                        return new LimitedGrid3(50_000, 50_000, 175, 10_000, 10_000, 300);
                    }
                },
            new IdProvider());

            AltAsync.Log("Server started.");
            _moduleController.Load();
        }

        public void Stop()
        {

        }
    }
}
