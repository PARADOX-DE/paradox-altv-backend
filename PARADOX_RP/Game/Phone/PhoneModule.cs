using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Phone.Interfaces;
using PARADOX_RP.Game.Phone.Models;
using PARADOX_RP.UI;
using PARADOX_RP.UI.Windows.Phone;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Phone
{
    class PhoneModule : ModuleBase<PhoneModule>
    {
        private IEnumerable<PhoneApplicationModel> _phoneApplications;
        public PhoneModule(PXContext pxContext, IEventController eventController, IEnumerable<IPhoneApplication> phoneApplications) : base("Phone")
        {
            _phoneApplications = LoadDatabaseTable<PhoneApplicationModel>(null, (p) =>
            {
                //todo: db table yk
            });

            eventController.OnClient<PXPlayer>("OpenPhone", OpenPhone);
        }

        private void OpenPhone(PXPlayer player)
        {
            if (!player.CanInteract() || !player.IsValid()) return;
            if (!player.HasPhone) return;

            WindowManager.Instance.Get<PhoneWindow>().Show(player);
        }
    }
}
