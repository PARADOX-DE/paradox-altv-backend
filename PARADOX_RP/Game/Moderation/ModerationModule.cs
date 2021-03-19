using Microsoft.EntityFrameworkCore;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PARADOX_RP.Game.Moderation
{
    class ModerationModule : ModuleBase<ModerationModule>
    {
        public ModerationModule() : base("Moderation") { }

        public async void BanPlayer(PXPlayer player)
        {
            player.Kick("Du wurdest gebannt. Für weitere Informationen melde dich im Support!");
            await using (var px = new PXContext())
            {
                BanList existingBanEntry = await px.BanList.FirstOrDefaultAsync(e => e.Player.Id == player.Id);
                if (existingBanEntry == null)  existingBanEntry.Active = true; 
                else
                {
                    BanList banEntry = new BanList(1, 0, false, DateTime.Now);
                    px.BanList.Add(banEntry);
                }

                await px.SaveChangesAsync();
            }
        }
    }
}
