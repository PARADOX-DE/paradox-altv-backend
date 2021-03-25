using AltV.Net;
using Microsoft.EntityFrameworkCore;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PARADOX_RP.Utils
{
    class Configuration
    {
        public static ServerConfig Instance { get; set; }

        public void LoadConfiguration()
        {
            using (var px = new PXContext())
            {
                ServerConfig config = px.ServerConfig.FirstOrDefault(i => i.PluginIdentifier == Alt.Core.Resource.Name);
                if (config == null) return;

                Instance = config;
            }
        }
    }
}
