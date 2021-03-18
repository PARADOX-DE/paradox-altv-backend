using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Administration.Models
{
    public class PermissionModel
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<PermissionAssignmentModel> PermissionSupportRankAssignment { get; set; }
    }
}
