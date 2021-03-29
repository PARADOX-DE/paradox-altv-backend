using AltV.Net;
using Microsoft.EntityFrameworkCore;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Game.Moderation;
using PARADOX_RP.Handlers.Login.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Handlers.Login
{
    class LoginHandler : ILoginHandler
    {
        public async Task<bool> LoadPlayer(PXPlayer player, string userName)
        {
            await using (var px = new PXContext())
            {

                Players dbPlayer = await px.Players.Include(p => p.SupportRank).ThenInclude(p => p.PermissionAssignments).ThenInclude(p => p.Permission)
                                                    .Include(p => p.PlayerClothes)
                                                    .Include(p => p.PlayerTeamData)
                                                    .Include(p => p.Team)
                                                    .FirstOrDefaultAsync(p => p.Username == userName);

                if (dbPlayer == null) return await Task.FromResult(false);
                player.LoggedIn = true;

                player.SqlId = dbPlayer.Id;
                player.Username = dbPlayer.Username;
                player.SupportRank = dbPlayer.SupportRank;

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
                }
                else
                {
                    Alt.Log("FraktionsData-Objekt existiert bereits.");

                }

                /**/

                //player.Clothes = _clothingDictionary;

                if (await ModerationModule.Instance.IsBanned(player))
                {
                    player.Kick("Du bist gebannt. Für weitere Informationen melde dich im Support!");
                    return await Task.FromResult(false);
                }

                return await Task.FromResult(true);
            }
        }
    }
}
