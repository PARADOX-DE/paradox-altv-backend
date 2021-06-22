using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    [Table("server_configuration")]
    public class ServerConfig
    {
        public int Id { get; set; }
        public string PluginIdentifier { get; set; }
        public string Version { get; set; }
        public bool DevMode { get; set; } = true;

        /* MODULE CONFIGURATIONS */
        // VehicleRadioModule
        public string VehicleRadioURL { get; set; }
    }
}
