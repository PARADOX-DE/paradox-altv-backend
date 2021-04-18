using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Controllers.Login.Interface
{
    interface ILoginController
    {
        Task<bool> CheckLogin(PXPlayer player, string userName, string password);
        Task<LoadPlayerResponse> LoadPlayer(PXPlayer player, string userName);

        Task SavePlayers();
    }
}
