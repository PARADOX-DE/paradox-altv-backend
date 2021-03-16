using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Utils
{
    class Configuration
    {
        public static Configuration Instance { get; } = new Configuration();

        public bool DevMode { get; set; } = true;
        public string Resource { get; set; } = string.Empty;
    }
}
