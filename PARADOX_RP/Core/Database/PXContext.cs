using AltV.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Game.Administration.Models;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Core.Database
{
    internal class PXContext : DbContext
    {

        public DbSet<Players> Players { get; set; }
        public DbSet<SupportRankModel> SupportRanks { get; set; }
        public DbSet<PermissionModel> Permissions { get; set; }
        public DbSet<PermissionAssignmentModel> PermissionAssignments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = $"Server=localhost;Database=altv-paradox_rp;Uid=root;Pwd=divan123;";
            optionsBuilder.UseMySql(connection, ServerVersion.AutoDetect(connection));
        }
    }
}
