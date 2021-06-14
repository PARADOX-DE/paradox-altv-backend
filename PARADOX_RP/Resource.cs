using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Interface;
using PARADOX_RP.Controllers;
using System.Threading.Tasks;
using PARADOX_RP.Core.Database;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
using AltV.Net.ColoredConsole;
using PARADOX_RP.Utils;
using PARADOX_RP.Utils.Interface;
using System;
using System.IO;

namespace PARADOX_RP
{
    public class Resource : AsyncResource
    {
        private IApplication _application;
        private ILogger _logger;

        public override void OnStart()
        {
            using (var px = new PXContext())
            {
                var databaseCreator = px.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                try
                {
                    databaseCreator.EnsureCreated();
                }
                catch { }
            }

            using var autofac = new PXContainer();
            autofac.RegisterTypes();
            autofac.ResolveTypes();

            _application = autofac.Resolve<IApplication>();
            _logger = autofac.Resolve<ILogger>();

            /*
             * 
             * START APPLICATIONS
             * 
             */

            _logger.Console(ConsoleLogType.SUCCESS, "Application", $"Starting {_application.Name}... // [by {_application.Author}]");
            _application.Start();
        }

        public override void OnStop()
        {

        }

        public override IEntityFactory<IPlayer> GetPlayerFactory()
        {
            return new PXPlayerFactory();
        }

        public override IEntityFactory<IVehicle> GetVehicleFactory()
        {
            return new PXVehicleFactory();
        }
    }
}
