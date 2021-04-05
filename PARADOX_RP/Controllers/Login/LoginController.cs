using AltV.Net;
using AltV.Net.Async;
using Microsoft.EntityFrameworkCore;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Game.Inventory;
using PARADOX_RP.Game.Moderation;
using PARADOX_RP.Handlers.Login.Interface;
using PARADOX_RP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Handlers.Login
{
    public enum LoadPlayerResponse
    {
        ABORT,
        NEW_PLAYER,
        SUCCESS
    }

    class LoginController : ILoginController
    {
        public async Task<bool> CheckLogin(PXPlayer player, string hashedPassword)
        {
            await using (var px = new PXContext())
            {
                string userName = await player.GetNameAsync();
                Players dbPlayer = await px.Players
                                       .FirstOrDefaultAsync(p => p.Username == userName);

                if (dbPlayer == null) return await Task.FromResult(false);

                try
                {
                    if (BCrypt.Net.BCrypt.Verify(hashedPassword, dbPlayer.Password))
                    {
                        if (Configuration.Instance.DevMode) Alt.Log($"[DEVMODE] {dbPlayer.Username} requested Login.");
                        return await Task.FromResult(true);
                    }
                }
                catch (BCrypt.Net.SaltParseException) { return await Task.FromResult(false); }
            }

            return await Task.FromResult(false);
        }

        public async Task<LoadPlayerResponse> LoadPlayer(PXPlayer player, string userName)
        {
            await using (var px = new PXContext())
            {
                Players dbPlayer = await px.Players
                                                    .Include(p => p.SupportRank).ThenInclude(p => p.PermissionAssignments).ThenInclude(p => p.Permission)
                                                    .Include(p => p.PlayerClothes)
                                                    .Include(p => p.PlayerTeamData)
                                                    .Include(p => p.Team)
                                                    .FirstOrDefaultAsync(p => p.Username == userName);

                if (dbPlayer == null) return await Task.FromResult(LoadPlayerResponse.ABORT);
                player.LoggedIn = true;

                player.SqlId = dbPlayer.Id;
                player.Username = dbPlayer.Username;
                player.SupportRank = dbPlayer.SupportRank;
                player.Team = dbPlayer.Team;

                /* New-Player Generation */
                if (dbPlayer.PlayerClothes.FirstOrDefault() == null)
                {
                    var playerClothesInsert = new PlayerClothes()
                    {
                        PlayerId = dbPlayer.Id
                    };

                    await px.PlayerClothes.AddAsync(playerClothesInsert);
                    await px.SaveChangesAsync();
                }
                else
                {
                    Alt.Log("Kleidungs-Objekt existiert bereits.");
                }

                if (dbPlayer.PlayerTeamData.FirstOrDefault() == null)
                {
                    var playerTeamDataInsert = new PlayerTeamData()
                    {
                        PlayerId = dbPlayer.Id
                    };

                    await px.PlayerTeamData.AddAsync(playerTeamDataInsert);
                    await px.SaveChangesAsync();

                    player.PlayerTeamData = playerTeamDataInsert;
                }
                else
                {
                    Alt.Log("FraktionsData-Objekt existiert bereits.");
                    player.PlayerTeamData = dbPlayer.PlayerTeamData.FirstOrDefault();
                }

                /**/

                //player.Clothes = _clothingDictionary;

                // InventoryModule.Instance.OpenInventory(player);

                if (await ModerationModule.Instance.IsBanned(player))
                {
                    await player.KickAsync("Du bist gebannt. Für weitere Informationen melde dich im Support!");
                    return await Task.FromResult(LoadPlayerResponse.ABORT);
                }

                Pools.Instance.Register(player.SqlId, player);

                if (dbPlayer.PlayerCustomization.FirstOrDefault() == null)
                {
                    return await Task.FromResult(LoadPlayerResponse.NEW_PLAYER);
                }

                await player.SpawnAsync(dbPlayer.Position);

                return await Task.FromResult(LoadPlayerResponse.SUCCESS);
            }
        }
    }
}
