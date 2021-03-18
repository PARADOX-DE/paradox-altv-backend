using PARADOX_RP.Core.Database.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Game.Administration.Models
{
    [Table("support_ranks")]
    public class SupportRankModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int Color_R { get; set; }
        public int Color_G { get; set; }
        public int Color_B { get; set; }

        public virtual ICollection<PermissionAssignmentModel> PermissionAssignments { get; set; }
    }
}
