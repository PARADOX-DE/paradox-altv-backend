using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Interface;
using PARADOX_RP.Handlers;

namespace PARADOX_RP
{
    public class Resource : AsyncResource
    {
        private ICoreSystem _coreSystem;
        public override void OnStart()
        {
            using var containerHelper = new PXContainer();
            containerHelper.RegisterTypes();
            containerHelper.ResolveTypes();

            _coreSystem = containerHelper.Resolve<ICoreSystem>();
            _coreSystem.Start();

            //EventHandler.Instance.Load();
        }

        public override void OnStop()
        {
            throw new System.NotImplementedException();
        }

        public override IEntityFactory<IPlayer> GetPlayerFactory()
        {
            return new PXPlayerFactory();
        }
    }
}
