using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Arrival.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Char
{
    public enum CharCreationType
    {
        NEW,
        EDIT
    }

    class CharModule : ModuleBase<CharModule>
    {

        public CharModule() : base("Char")
        {

        }

        public void CreatePlayerCharacter(PXPlayer player, CharCreationType charCreationType)
        {

        }
    }
}
