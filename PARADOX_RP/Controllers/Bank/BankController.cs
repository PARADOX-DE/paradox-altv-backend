using PARADOX_RP.Controllers.Bank.Interface;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Controllers.Bank
{
    class BankController : IBankController
    {
        public async Task CreateBankHistory(PXPlayer player, string Name, BankActionTypes Action, int Amount)
        {
            await using(var px = new PXContext())
            {
                await px.BankHistory.AddAsync(new PlayerBankHistory()
                {
                    PlayerId = player.SqlId,
                    Name = Name,
                    Date = DateTime.Now,
                    Action = Action,
                    Money = Amount
                });

                await px.SaveChangesAsync();
            }
        }
    }
}
