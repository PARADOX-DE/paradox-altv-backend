using AltV.Net.Async;
using PARADOX_RP.Controllers.Inventory;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Events;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Job.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Farming
{
    public sealed class JobModule : Module<JobModule>, IEventModuleLoad
    {
        private readonly PXContext _pxContext;
        private readonly IEnumerable<IJob> _jobs;
        private readonly IInventoryController _inventoryController;

        public JobModule(PXContext pxContext, IEnumerable<IJob> jobs, IInventoryController inventoryController) : base("Farming")
        {
            _pxContext = pxContext;
            _jobs = jobs;
            _inventoryController = inventoryController;
        }

        public void OnModuleLoad()
        {
            _jobs.ForEach((j) =>
            {
                AltAsync.Log(j.GetType().Name);
                AltAsync.Log(j.GetType().FullName);
            });
        }
    }
}
