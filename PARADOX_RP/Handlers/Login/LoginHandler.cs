﻿using Microsoft.EntityFrameworkCore;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Handlers.Login.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Handlers.Login
{
    class LoginHandler : ILoginHandler
    {
        public async Task<bool> LoadPlayer(PXPlayer player)
        {
            await using (var px = new PXContext())
            {

                Players dbPlayer = await px.Players.Include(p => p.SupportRank).ThenInclude(p => p.PermissionAssignments).ThenInclude(p => p.Permission).
                                                    FirstOrDefaultAsync(p => p.Username == player.Name);

                if(dbPlayer == null) return await Task.FromResult(false);
                player.LoggedIn = true;

                player.SqlId = dbPlayer.Id;
                player.Username = dbPlayer.Username;
                player.SupportRank = dbPlayer.SupportRank;

                return await Task.FromResult(true);
            }
        }
    }
}