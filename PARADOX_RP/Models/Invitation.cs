using PARADOX_RP.Core.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Models
{
    public class Invitation
    {
        public int InviterId { get; set; }
        public Teams Team { get; set; }
    }
}
