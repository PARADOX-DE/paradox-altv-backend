using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Char;
using PARADOX_RP.Game.Misc.Progressbar;
using PARADOX_RP.Game.Misc.Progressbar.Extensions;
using PARADOX_RP.Game.Moderation;
using PARADOX_RP.Controllers.Login;
using PARADOX_RP.Controllers.Login.Interface;
using PARADOX_RP.UI;
using PARADOX_RP.UI.Windows;
using PARADOX_RP.Utils;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Game.Login.Extensions;

namespace PARADOX_RP.Game.Login
{
    class LoginModule : ModuleBase<LoginModule>
    {
        private readonly IEventController _eventController;
        private readonly ILoginController _loginHandler;

        public LoginModule(IEventController eventController, ILoginController loginHandler) : base("Login")
        {
            _loginHandler = loginHandler;
            _eventController = eventController;

            _eventController.OnClient<PXPlayer, string, string>("RequestLoginResponse", RequestLoginResponse);
        }

        private Position _loginPosition = new Position(3486.3296f, 3712.8264f, 57.2843f);

        public override void OnModuleLoad()
        {

        }

        public override async void OnPlayerConnect(PXPlayer player)
        {
            player.Model = (uint)PedModel.FreemodeMale01;
            await player.SpawnAsync(_loginPosition);
            
            //BCrypt.Net.BCrypt.has
            /*if (Configuration.Instance.DevMode)
            {
                LoadPlayerResponse loadPlayerResponse = await _loginHandler.LoadPlayer(player, player.Name);
                if (loadPlayerResponse == LoadPlayerResponse.ABORT) return;
                else
                {
                    if (loadPlayerResponse == LoadPlayerResponse.NEW_PLAYER)
                    {
                        CharModule.Instance.CreatePlayerCharacter(player, CharCreationType.NEW);
                        return;
                    }

                    return;
                }
            }*/

            WindowController.Instance.Get<LoginWindow>().Show(player);
        }

        public async void RequestLoginResponse(PXPlayer player, string username, string hashedPassword)
        {
            if (player.LoggedIn) return;

            if (await _loginHandler.CheckLogin(player, username, hashedPassword))
            {
                LoadPlayerResponse loadPlayerResponse = await _loginHandler.LoadPlayer(player, username);
                if (loadPlayerResponse == LoadPlayerResponse.ABORT) return;
                else
                {

                    // HANDLE EVERYTHING AFTER LOAD PLAYER
                    if (loadPlayerResponse == LoadPlayerResponse.NEW_PLAYER)
                    {
                        WindowController.Instance.Get<LoginWindow>().Hide(player);

                        CharModule.Instance.CreatePlayerCharacter(player, CharCreationType.NEW);
                        return;
                    }
                }
            }
        }
    }
}
