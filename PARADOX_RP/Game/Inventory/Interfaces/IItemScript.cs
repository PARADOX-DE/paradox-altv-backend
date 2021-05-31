using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Inventory.Interfaces
{
    public interface IItemScript
    {
        int Id { get; }
        string ScriptName { get; }
        Task<bool> UseItem();
    }
}
