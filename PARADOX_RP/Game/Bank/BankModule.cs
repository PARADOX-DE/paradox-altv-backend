using AltV.Net.Async;
using AltV.Net.Data;
using EntityStreamer;
using Newtonsoft.Json;
using PARADOX_RP.Controllers.Bank.Interface;
using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Events;
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
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Bank
{
    class BankModule : ModuleBase<BankModule>, IEventKeyPressed, IEventPlayerConnect, IEventModuleLoad
    {
        private readonly string _bankName = "N26 Bank";
        private Dictionary<int, BankATMs> _BankATMs = new Dictionary<int, BankATMs>();

        private readonly IBankController _bankController;

        public BankModule(PXContext px, IBankController bankController, IEventController eventController) : base("Bank")
        {
            _bankController = bankController;

            LoadDatabaseTable(px.BankATMs, (BankATMs atm) =>
            {
                _BankATMs.Add(atm.Id, atm);
            });

            eventController.OnClient<PXPlayer, int>("DepositMoney", DepositMoney);
            eventController.OnClient<PXPlayer, int>("WithdrawMoney", WithdrawMoney);
            eventController.OnClient<PXPlayer, string, int>("TransferMoney", TransferMoney);
        }

        public void OnPlayerConnect(PXPlayer player)
        {
            if (Configuration.Instance.DevMode)
            {
                _BankATMs.ForEach((atm) =>
                {
                    player.AddBlips($"Bankautomat #{atm.Key}", atm.Value.Position, 108, 25, 1, true);

                    MarkerStreamer.Create(MarkerTypes.MarkerTypeDallorSign, Vector3.Add(atm.Value.Position, new Vector3(0, 0, 1)), new Vector3(1, 1, 1), null, null, new Rgba(37, 165, 202, 200));
                });
            }
        }

        public void OnModuleLoad()
        {
            if (Configuration.Instance.DevMode)
                _BankATMs.ForEach((atm) => MarkerStreamer.Create(MarkerTypes.MarkerTypeDallorSign, Vector3.Add(atm.Value.Position, new Vector3(0, 0, 1)), new Vector3(1, 1, 1), null, null, new Rgba(37, 165, 202, 200)));
        }

        public Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key)
        {
            if (key == KeyEnumeration.E)
            {
                BankATMs targetATM = _BankATMs.Values.FirstOrDefault(a => a.Position.Distance(player.Position) < 3);
                if (targetATM == null) return Task.FromResult(false);


                WindowController.Instance.Get<BankWindow>().Show(player, new BankWindowWriter(player.Username, player.Money, player.BankMoney, player.PlayerBankHistory.Take(50).ToList()));
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public async void DepositMoney(PXPlayer player, int moneyAmount)
        {
            if (!player.IsValid()) return;
            if (!player.CanInteract()) return;
            if (!WindowController.Instance.Get<BankWindow>().IsVisible(player)) return;

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
            player.SendNotification(_bankName, $"Sie haben erfolgreich {moneyAmount} $ eingezahlt.", NotificationTypes.SUCCESS);

            await _bankController.CreateBankHistory(player, "Bargeldeinzahlung", BankActionTypes.DEPOSIT, moneyAmount);
        }

        public async void WithdrawMoney(PXPlayer player, int moneyAmount)
        {
            if (!player.IsValid()) return;
            if (!player.CanInteract()) return;
            if (!WindowController.Instance.Get<BankWindow>().IsVisible(player)) return;

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
            player.SendNotification(_bankName, $"Sie haben erfolgreich {moneyAmount} $ ausgezahlt.", NotificationTypes.SUCCESS);

            await _bankController.CreateBankHistory(player, "Bargeldauszahlung", BankActionTypes.WITHDRAW, moneyAmount);
        }

        private async void TransferMoney(PXPlayer player, string targetString, int moneyAmount)
        {
            if (!player.IsValid()) return;
            if (!player.CanInteract()) return;
            if (!WindowController.Instance.Get<BankWindow>().IsVisible(player)) return;

            if (player.Username.ToLower() == targetString.ToLower())
            {
                player.SendNotification(_bankName, "Du kannst dir nicht selber Geld senden!", NotificationTypes.ERROR);
                //TODO: add log
                return;
            }

            if (player.BankMoney < moneyAmount)
            {
                player.SendNotification(_bankName, "Du hast nicht genügend Geld auf dem Konto!", NotificationTypes.ERROR);
                return;
            }

            PXPlayer target = Pools.Instance.Get<PXPlayer>(PoolType.PLAYER).FirstOrDefault(p => p.Username.ToLower() == targetString.ToLower());
            if (target == null || !target.IsValid())
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

            player.SendNotification(_bankName, $"Sie haben erfolgreich {moneyAmount} $ an {targetString} überwiesen.", NotificationTypes.SUCCESS);
            target.SendNotification(_bankName, $"Sie haben eine Überweisung erhalten!", NotificationTypes.SUCCESS);
            await _bankController.CreateBankHistory(player, target.Username, BankActionTypes.TRANSFER_SENT, moneyAmount);
            await _bankController.CreateBankHistory(target, player.Username, BankActionTypes.TRANSFER_RECEIVED, moneyAmount);
        }
    }
}
