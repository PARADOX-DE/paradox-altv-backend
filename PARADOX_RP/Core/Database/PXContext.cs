using AltV.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
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
        public DbSet<PlayerCustomization> PlayerCustomization { get; set; }
        public DbSet<PlayerClothesWearing> PlayerClothesWearing { get; set; }
        public DbSet<PlayerInjuryData> PlayerInjuryData { get; set; }
        public DbSet<PlayerTeamData> PlayerTeamData { get; set; }
        public DbSet<PlayerPhoneSettings> PlayerPhoneSettings { get; set; }
        public DbSet<SupportRankModel> SupportRanks { get; set; }
        public DbSet<PermissionModel> Permissions { get; set; }
        public DbSet<PermissionAssignmentModel> PermissionAssignments { get; set; }
        public DbSet<BankATMs> BankATMs { get; set; }
        public DbSet<Clothes> Clothes { get; set; }

        public DbSet<Shops> Shops { get; set; }
        public DbSet<ShopItems> ShopItems { get; set; }

        public DbSet<Vehicles> Vehicles { get; set; }
        public DbSet<VehicleClass> VehicleClass { get; set; }
        public DbSet<VehicleShops> VehicleShops { get; set; }
        public DbSet<VehicleShopsContent> VehicleShopsContent { get; set; }

        public DbSet<Garages> Garages { get; set; }
        public DbSet<GarageSpawns> GarageSpawns { get; set; }
        public DbSet<GasStations> GasStations { get; set; }
        public DbSet<GasStationPetrols> GasStationPetrols { get; set; }
        public DbSet<Injuries> Injuries { get; set; }
        public DbSet<Items> Items { get; set; }
        public DbSet<Eastereggs> Eastereggs { get; set; }
        public DbSet<Teams> Teams { get; set; }
        public DbSet<PositionList> PositionList { get; set; }
        public DbSet<Jumppoints> JumpPoints { get; set; }

        public DbSet<Inventories> Inventories { get; set; }
        public DbSet<InventoryInfo> InventoryInfo { get; set; }
        public DbSet<InventoryItemAssignments> InventoryItemAssignments { get; set; }
        public DbSet<InventoryItemSignatures> InventoryItemSignatures { get; set; }

        public DbSet<BanList> BanList { get; set; }
        public DbSet<ServerConfig> ServerConfig { get; set; }

        public static readonly ILoggerFactory loggerFactory =
           LoggerFactory.Create(
                builder =>
                {
                    builder.AddConsole();
                }
        );

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = $"Server=127.0.0.1; port=3306; Database=altv-paradox_rp; UserId=root; Password=lCvLpEGKhvz4WDsN;";
            optionsBuilder.UseMySql(connection, ServerVersion.AutoDetect(connection));
            //optionsBuilder.UseLoggerFactory(loggerFactory);
        }
    }
}
