using AltV.Net.Async;
using Microsoft.EntityFrameworkCore;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Moderation
{
    class ModerationModule : ModuleBase<ModerationModule>
    {
        public ModerationModule() : base("Moderation") { }

        public async Task BanPlayer(PXPlayer player, PXPlayer moderator)
        {
            await using (var px = new PXContext())
            {
                BanList existingBanEntry = await px.BanList.FirstOrDefaultAsync(e => e.Player.Id == player.Id);
                if (existingBanEntry != null) existingBanEntry.Active = true; 
                else
                {
                    BanList banEntry = new BanList(player.SqlId, moderator.SqlId, true, DateTime.Now);
                    await px.BanList.AddAsync(banEntry);
                }

                await px.SaveChangesAsync();
            }

            await player.KickAsync("Du wurdest gebannt. Für weitere Informationen melde dich im Support!");
        }
    }
}
