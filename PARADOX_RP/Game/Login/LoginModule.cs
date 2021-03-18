using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Handlers.Login.Interface;
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

namespace PARADOX_RP.Game.Login
{
    class LoginModule : ModuleBase<LoginModule>
    {
        private readonly ILoginHandler _loginHandler;

        public LoginModule(ILoginHandler loginHandler) : base("Login")
        {
            _loginHandler = loginHandler;
        }

        public override void OnModuleLoad()
        {

        }

        public override async void OnPlayerConnect(PXPlayer player)
        {

            player.Model = (uint)PedModel.FreemodeMale01;
            await player.SpawnAsync(new Position(0, 0, 72));

            if (Configuration.Instance.DevMode)
            {
                await _loginHandler.LoadPlayer(player, "Walid");
                return;
            }

            WindowManager.Instance.Get<LoginWindow>().Show(player, JsonConvert.SerializeObject(new LoginWindowObject() { name = player.Name }));
        }

        public async void RequestLoginResponse(PXPlayer player, string hashedPassword)
        {
            if (player.LoggedIn) return;

            await using (var px = new PXContext())
            {
                Players dbPlayer = await px.Players.Include(p => p.SupportRank).ThenInclude(p => p.PermissionAssignments).
                                                    FirstOrDefaultAsync(p => p.Username == player.Name);

                if (dbPlayer.Password == hashedPassword)
                {
                    //dbPlayer.
                }
            }

            //SEND RESPONSE TO PLAYER
        }
    }
}
