using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Phone.Interfaces;
using PARADOX_RP.Game.Phone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Phone
{
    class PhoneModule : ModuleBase<PhoneModule>
    {
        private IEnumerable<PhoneApplicationModel> _phoneApplications;
        public PhoneModule(PXContext pxContext, IEnumerable<IPhoneApplication> phoneApplications) : base("Phone")
        {
            _phoneApplications = LoadDatabaseTable<PhoneApplicationModel>(null, (p) =>
            {
                //todo: db table yk
            });
        }

    }
}
