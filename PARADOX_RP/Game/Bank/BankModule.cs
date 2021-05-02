using AltV.Net.Async;
using EntityStreamer;
using Newtonsoft.Json;
using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.UI;
using PARADOX_RP.UI.Windows;
using PARADOX_RP.Utils;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Bank
{
    class BankModule : ModuleBase<BankModule>
    {
        private Dictionary<int, BankATMs> _BankATMs = new Dictionary<int, BankATMs>();

        public BankModule(PXContext px, IEventController eventController) : base("Bank")
        {
            px.BankATMs.ForEach(b =>
            {
                _BankATMs.Add(b.Id, b);
            });

            eventController.OnClient<PXPlayer, int>("DepositMoney", DepositMoney);
            eventController.OnClient<PXPlayer, int>("WithdrawMoney", WithdrawMoney);
            eventController.OnClient<PXPlayer, string, int>("TransferMoney", TransferMoney);

        }


        private readonly string _bankName = "N26 Bank";
        
        public override Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key)
        {
            if (key == KeyEnumeration.E)
            {
                BankATMs targetATM = _BankATMs.Values.FirstOrDefault(a => a.Position.Distance(player.Position) < 3);
                if (targetATM == null) return Task.FromResult(false);

                WindowManager.Instance.Get<BankWindow>().Show(player, JsonConvert.SerializeObject(new BankWindowObject(player.Username)));
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public async void DepositMoney(PXPlayer player, int moneyAmount)
        {
            if (!player.IsValid()) return;
            if (!player.CanInteract()) return;
            if (!WindowManager.Instance.Get<BankWindow>().IsVisible(player)) return;

            if (!await player.TakeMoney(moneyAmount))
            {
                player.SendNotification(_bankName, "Du hast nicht genügend Geld dabei.", NotificationTypes.ERROR);
                return;
            }

            await using (var px = new PXContext())
            {
                player.BankMoney += moneyAmount;
                (await px.Players.FindAsync(player.SqlId)).BankMoney = player.BankMoney;
                await px.SaveChangesAsync();
            }
        }

        public async void WithdrawMoney(PXPlayer player, int moneyAmount)
        {
            if (!player.IsValid()) return;
            if (!player.CanInteract()) return;
            if (!WindowManager.Instance.Get<BankWindow>().IsVisible(player)) return;

            if (player.BankMoney < moneyAmount)
            {
                player.SendNotification(_bankName, "Du hast nicht genügend Geld auf dem Konto!", NotificationTypes.ERROR);
                return;
            }

            await using (var px = new PXContext())
            {

                player.BankMoney -= moneyAmount;
                (await px.Players.FindAsync(player.SqlId)).BankMoney = player.BankMoney;
                await px.SaveChangesAsync();
            }

            await player.AddMoney(moneyAmount);
        }
        private async void TransferMoney(PXPlayer player, string targetString, int moneyAmount)
        {
            if (!player.IsValid()) return;
            if (!player.CanInteract()) return;
            if (!WindowManager.Instance.Get<BankWindow>().IsVisible(player)) return;

            if (player.BankMoney < moneyAmount)
            {
                player.SendNotification(_bankName, "Du hast nicht genügend Geld auf dem Konto!", NotificationTypes.ERROR);
                return;
            }

            PXPlayer target = Pools.Instance.Get<PXPlayer>(PoolType.PLAYER).FirstOrDefault(p => p.Username == targetString);
            if(target == null || !target.IsValid())
            {
                player.SendNotification(_bankName, $"Es wurde kein Konto mit dem Besitzer {targetString} gefunden!", NotificationTypes.ERROR);
                return;
            }

            await using (var px = new PXContext())
            {
                player.BankMoney -= moneyAmount;
                target.BankMoney += moneyAmount;

                Players dbPlayer = await px.Players.FindAsync(player.SqlId);
                Players dbTargetPlayer = await px.Players.FindAsync(target.SqlId);

                dbPlayer.BankMoney = player.BankMoney;
                dbTargetPlayer.BankMoney = target.BankMoney;

                await px.SaveChangesAsync();
            }
        }
    }
}
