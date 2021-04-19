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
        private Dictionary<int, PhoneApplicationModel> _phoneApplications;
        public PhoneModule(IEnumerable<IPhoneApplication> phoneApplications) : base("Phone")
        {
            _phoneApplications = new Dictionary<int, PhoneApplicationModel>();
        }

    }
}
