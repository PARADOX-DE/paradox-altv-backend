using PARADOX_RP.Core.Factories;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Controllers.Bank.Interface
{
    interface IBankController
    {
        Task CreateBankHistory(PXPlayer player, string Name, BankActionTypes Action, int Amount);
    }
}
