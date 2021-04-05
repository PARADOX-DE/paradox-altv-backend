using AltV.Net.Async;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Arrival.Extensions;
using PARADOX_RP.UI;
using PARADOX_RP.UI.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PARADOX_RP.Game.Char
{
    public enum CharCreationType
    {
        NEW,
        EDIT
    }

    class CharModule : ModuleBase<CharModule>
    {

        public CharModule() : base("Char")
        {

            AltAsync.OnClient<PXPlayer, string, string, string, string>("SavePlayerCharacter", SavePlayerCharacter);
        }

        public void CreatePlayerCharacter(PXPlayer player, CharCreationType charCreationType)
        {
            WindowManager.Instance.Get<CharCreationWindow>().Show(player);
        }

        public async void SavePlayerCharacter(PXPlayer player, string firstName, string lastName, string birthDate, string customizationString)
        {
            if (!player.LoggedIn) return;
            if (player.PlayerCustomization != null) return;

            await using(var px = new PXContext())
            {
                PlayerCustomization dbPlayerCustomization = await px.PlayerCustomization.Where(p => p.PlayerId == player.SqlId).FirstOrDefaultAsync();
                if (dbPlayerCustomization == null) return;

                dbPlayerCustomization.Customization = customizationString;
            }
        }
    }
}
