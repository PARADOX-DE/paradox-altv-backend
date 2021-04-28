using AltV.Net.Async;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Arrival;
using PARADOX_RP.UI;
using PARADOX_RP.UI.Windows;
using PARADOX_RP.Utils.Enums;
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

        public CharModule(IEventController eventController) : base("Char")
        {

            eventController.OnClient<PXPlayer, string, string, string, string>("SavePlayerCharacter", SavePlayerCharacter);
        }

        public void CreatePlayerCharacter(PXPlayer player, CharCreationType charCreationType)
        {
            WindowManager.Instance.Get<CharCreationWindow>().Show(player);
        }

        public async void SavePlayerCharacter(PXPlayer player, string firstName, string lastName, string birthDate, string customizationString)
        {
            if (!player.LoggedIn) return;
            if (!WindowManager.Instance.Get<CharCreationWindow>().IsVisible(player)) return;
            WindowManager.Instance.Get<CharCreationWindow>().Hide(player);

            await using (var px = new PXContext())
            {
                PlayerCustomization dbPlayerCustomization = await px.PlayerCustomization.Where(p => p.PlayerId == player.SqlId).FirstOrDefaultAsync();
                if (dbPlayerCustomization == null)
                {
                    dbPlayerCustomization = new PlayerCustomization()
                    {
                        PlayerId = player.SqlId,
                        Customization = customizationString
                    };
                    
                    foreach (var arrivalClothing in ArrivalModule.Instance._arrivalClothes)
                    {
                        //TODO: do the same for females 
                        if (arrivalClothing.Value.Gender == (int)Gender.MALE)
                        {
                            PlayerClothesWearing alreadyExistingCloth = await px.PlayerClothesWearing.FirstOrDefaultAsync(i => (i.PlayerId == player.SqlId) && i.ComponentVariation == arrivalClothing.Key);
                            if (alreadyExistingCloth != null)
                            {
                                alreadyExistingCloth.ClothingId = arrivalClothing.Value.Id;
                            }
                            else
                            {
                                var clothingToInsert = new PlayerClothesWearing()
                                {
                                    PlayerId = player.SqlId,
                                    ComponentVariation = arrivalClothing.Key,
                                    ClothingId = arrivalClothing.Value.Id
                                };

                                px.PlayerClothesWearing.Add(clothingToInsert);
                            }
                            player.Clothes[arrivalClothing.Key] = arrivalClothing.Value;
                            await player.SetClothes((int)arrivalClothing.Key, arrivalClothing.Value.Drawable, arrivalClothing.Value.Texture);
                        }
                    }

                    await px.PlayerCustomization.AddAsync(dbPlayerCustomization);
                    await px.SaveChangesAsync();
                }
                else
                {
                    dbPlayerCustomization.Customization = customizationString;
                    await px.SaveChangesAsync();
                }
            }

            await ArrivalModule.Instance.NewPlayerArrival(player);
        }
    }
}
