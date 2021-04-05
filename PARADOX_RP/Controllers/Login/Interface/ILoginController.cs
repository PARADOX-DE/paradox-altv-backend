using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Handlers.Login.Interface
{
    interface ILoginController
    {
        Task<bool> CheckLogin(PXPlayer player, string password);
        Task<LoadPlayerResponse> LoadPlayer(PXPlayer player, string userName);
    }
}
