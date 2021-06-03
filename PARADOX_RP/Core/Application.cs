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
        private readonly IWindowController _componentManager;
        public Application(IModuleController moduleController, IWindowController componentManager)
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

            AltAsync.Log("                                        ");
            AltAsync.Log("          ..............                ");
            AltAsync.Log("         .ck0000000000000ko;.           ");
            AltAsync.Log("           ,oooooooooooodkXW0:          ");
            AltAsync.Log("             ............ 'kWNc         ");
            AltAsync.Log("            lXXXXXXXXXXXk. ;XMx.        ");
            AltAsync.Log("           ;KM0lcccccccc,  oWWo         ");
            AltAsync.Log("          .xMWl .,;;;;;;;ckNNd.         ");
            AltAsync.Log("           cKWKldNMWWWWWWX0d,           ");
            AltAsync.Log("            .lKWWMXl,,,,,.              ");
            AltAsync.Log("              .lKWo                     ");
            AltAsync.Log("                .;.                     ");
            AltAsync.Log("                                        ");
            AltAsync.Log("                  © by PARADOX Roleplay.");
            AltAsync.Log("                                        ");

            _moduleController.Load();
        }

        public void Stop()
        {

        }
    }
}
