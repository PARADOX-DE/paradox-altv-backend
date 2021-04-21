using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Phone.Models
{
    class PhoneApplicationModel
    {
        public int Id { get; set; }
        public string ApplicationName { get; set; }
        public PhoneApplicationAlignments Alignment { get; set; }
    }
}
