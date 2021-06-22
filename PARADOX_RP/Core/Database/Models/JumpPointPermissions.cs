using AltV.Net.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    [Table("jumppoint_permissions")]
    public class JumpPointPermissions
    {
        public int Id { get; set; }
        public int JumpPointId { get; set; }
        public int TeamId { get; set; }

        public virtual JumpPoints JumpPoint { get; set; }
        public virtual Teams Team { get; set; }
    }
}
