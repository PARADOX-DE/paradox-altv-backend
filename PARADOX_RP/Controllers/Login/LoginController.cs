using AltV.Net;
using AltV.Net.Async;
using Microsoft.EntityFrameworkCore;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Game.Inventory;
using PARADOX_RP.Game.Moderation;
using PARADOX_RP.Controllers.Login.Interface;
using PARADOX_RP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PARADOX_RP.Game.Login.Extensions;
using PARADOX_RP.Game.Arrival;
using PARADOX_RP.Utils.Enums;
using PARADOX_RP.Game.Clothing;

namespace PARADOX_RP.Controllers.Login
{
    public enum LoadPlayerResponse
    {
        ABORT,
        NEW_PLAYER,
        SUCCESS
    }

    class LoginController : ILoginController
    {
        public async Task<bool> CheckLogin(PXPlayer player, string userName, string hashedPassword)
        {
            await using (var px = new PXContext())
            {
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
                catch (BCrypt.Net.SaltParseException)
                {
                    if (Configuration.Instance.DevMode) Alt.Log($"[DEVMODE] {dbPlayer.Username} threw SaltParseException.");
                    return await Task.FromResult(false);
                }
            }

            return await Task.FromResult(false);
        }

        public async Task<LoadPlayerResponse> LoadPlayer(PXPlayer player, string userName)
        {
            try
            {
                await using (var px = new PXContext())
                {
                    Players dbPlayer = await px.Players
                                                        .Include(p => p.SupportRank).ThenInclude(p => p.PermissionAssignments).ThenInclude(p => p.Permission)
                                                        .Include(p => p.PlayerClothes).ThenInclude(p => p.Clothing)
                                                        .Include(p => p.PlayerTeamData)
                                                        .Include(p => p.Team)
                                                        .Include(p => p.PlayerCustomization)
                                                        .FirstOrDefaultAsync(p => p.Username == userName);

                    if (dbPlayer == null) return await Task.FromResult(LoadPlayerResponse.ABORT);
                    player.LoggedIn = true;

                    player.SqlId = dbPlayer.Id;
                    player.Username = dbPlayer.Username;
                    player.SupportRank = dbPlayer.SupportRank;
                    player.Money = dbPlayer.Money;
                    player.BankMoney = dbPlayer.BankMoney;
                    player.Team = dbPlayer.Team;

                    /* New-Player Generation */

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

                    await player?.EmitAsync("ApplyPlayerCharacter", dbPlayer.PlayerCustomization.FirstOrDefault().Customization);
                    
                    Dictionary<ComponentVariation, Clothes> wearingClothes = new Dictionary<ComponentVariation, Clothes>();
                    foreach (PlayerClothesWearing playerClothesWearing in dbPlayer.PlayerClothes)
                    {
                        wearingClothes[playerClothesWearing.ComponentVariation] = playerClothesWearing.Clothing;
                        await player.SetClothes((int)playerClothesWearing.ComponentVariation, playerClothesWearing.Clothing.Drawable, playerClothesWearing.Clothing.Texture);
                    }

                    player.Clothes = wearingClothes;

                    await player?.PreparePlayer(dbPlayer.Position);

                    return await Task.FromResult(LoadPlayerResponse.SUCCESS);

                }
            }
            catch (Exception e) { Alt.Log("Failed to load player | " + e.Message); }
            return await Task.FromResult(LoadPlayerResponse.ABORT);
        }
           
        public async Task SavePlayers()
        {
            foreach(PXPlayer player in Pools.Instance.Get<PXPlayer>(PoolType.PLAYER))
            {
                await using(var px = new PXContext())
                {
                    Players dbPlayer = await px.Players.FindAsync(player.SqlId);
                    if (dbPlayer == null) continue;

                    if(player.Dimension == 0)
                    {
                        dbPlayer.Position_X = player.Position.X;
                        dbPlayer.Position_Y = player.Position.Y;
                        dbPlayer.Position_Z = player.Position.Z;
                    }

                    await px.SaveChangesAsync();
                }
            }
        }
    }
}
