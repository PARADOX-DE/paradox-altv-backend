using PARADOX_RP.Game.Phone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    public class PhoneApplications
    {
        public int Id { get; set; }
        public string ApplicationName { get; set; }
        public PhoneApplicationAlignments Alignment { get; set; }
    }
}
