using AltV.Net.Async;
using EntityStreamer;
using Newtonsoft.Json;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.UI;
using PARADOX_RP.UI.Windows;
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
        private readonly Dictionary<int, BankATMs> _BankATMs;

        public BankModule(PXContext pxContext) : base("Bank")
        {
            LoadDatabaseTable(pxContext.BankATMs, (BankATMs atm) =>
            {
                _BankATMs.Add(atm.Id, atm);
            });

            AltAsync.OnClient<PXPlayer, int>("DepositMoney", DepositMoney);
            AltAsync.OnClient<PXPlayer, int>("WithdrawMoney", WithdrawMoney);
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
    }
}
