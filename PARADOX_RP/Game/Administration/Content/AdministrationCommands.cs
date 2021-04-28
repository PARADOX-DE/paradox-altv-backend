using AltV.Net;
using Newtonsoft.Json;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Game.Commands;
using PARADOX_RP.Game.Commands.Attributes;
using PARADOX_RP.UI;
using PARADOX_RP.UI.Windows;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Administration.Content
{
    public class AdministrationCommands : ICommand
    {
        [Command("sendconfirmation")]
        public void SendConfirmation(PXPlayer player, string Title, string Description)
        {
            Alt.Log("works");
            if (!player.IsValid()) return;

            WindowManager.Instance.Get<ConfirmationWindow>().Show(player, new ConfirmationWindowWriter(Title, Description, "", ""));
        }
    }
}
