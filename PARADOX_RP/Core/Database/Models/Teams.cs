using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    public enum TeamTypes
    {
        NEUTRAL,
        STATE,
        BAD
    }

    public class Teams
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public TeamTypes TeamType { get; set; }
    }
}
