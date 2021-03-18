using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Game.Administration.Models
{
    [Table("permission_assignments")]
    public class PermissionAssignmentModel
    {
        public int Id { get; set; }
        public SupportRankModel SupportRank { get; set; }
        public PermissionModel Permission { get; set; }
    }
}
