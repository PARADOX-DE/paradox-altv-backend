using Microsoft.EntityFrameworkCore;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Game.Moderation;
using PARADOX_RP.Handlers.Login.Interface;
using System;
using System.Collections.Generic;
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
                                                    .FirstOrDefaultAsync(p => p.Username == userName);

                if (dbPlayer == null) return await Task.FromResult(false);
                player.LoggedIn = true;

                player.SqlId = dbPlayer.Id;
                player.Username = dbPlayer.Username;
                player.SupportRank = dbPlayer.SupportRank;

                Dictionary<int, Clothes> _clothingDictionary = new Dictionary<int, Clothes>();
                _clothingDictionary.Add(1, dbPlayer.PlayerClothes.Mask);
                _clothingDictionary.Add(3, dbPlayer.PlayerClothes.Torso);
                _clothingDictionary.Add(4, dbPlayer.PlayerClothes.Legs);
                _clothingDictionary.Add(5, dbPlayer.PlayerClothes.Bag);
                _clothingDictionary.Add(6, dbPlayer.PlayerClothes.Shoe);
                _clothingDictionary.Add(7, dbPlayer.PlayerClothes.Accessoire);
                _clothingDictionary.Add(8, dbPlayer.PlayerClothes.Undershirt);
                _clothingDictionary.Add(9, dbPlayer.PlayerClothes.Armor);
                _clothingDictionary.Add(10, dbPlayer.PlayerClothes.Decal);
                _clothingDictionary.Add(11, dbPlayer.PlayerClothes.Top);

                player.Clothes = _clothingDictionary;

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
