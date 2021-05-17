using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Misc.Position
{
    //TODO: after database entry => add to enumeration
    public enum Positions
    {
        NULL_POINT,
        MEDICAL_DEPARTMENT
    }


    class PositionModule : ModuleBase<PositionModule>
    {
        public Dictionary<Positions, AltV.Net.Data.Position> _positions = new Dictionary<Positions, AltV.Net.Data.Position>();
        public PositionModule(PXContext pxContext) : base("Position")
        {
            LoadDatabaseTable<PositionList>(pxContext.PositionList, (pos) =>
            {
                //Enum.TryParse<Positions>(pos.Id, out Positions positionEnum);
                if (!Enum.IsDefined(typeof(Positions), pos.Id)) return;
                _positions.Add((Positions)pos.Id, pos.Position);
            });
        }

        public AltV.Net.Data.Position Get(Positions position)
        {
            if (_positions.TryGetValue(position, out AltV.Net.Data.Position pos))
                return pos;

            return new AltV.Net.Data.Position(0, 0, 0);
        }
    }
}
