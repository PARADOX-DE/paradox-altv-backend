using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Handlers.Login.Interface
{
    interface ILoginHandler
    {
        Task<bool> LoadPlayer(PXPlayer player, string userName);
    }
}
