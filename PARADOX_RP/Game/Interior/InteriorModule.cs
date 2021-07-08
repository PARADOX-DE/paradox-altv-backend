using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Interior
{
    public sealed class InteriorModule : Module<InteriorModule>
    {
        private readonly PXContext _pxContext;

        private readonly Dictionary<int, Interiors> _interiors = new Dictionary<int, Interiors>();

        public InteriorModule(PXContext pxContext) : base("Interior")
        {
            _pxContext = pxContext;

            LoadDatabaseTable<Interiors>(_pxContext.Interiors, (i) => _interiors.Add(i.Id, i));
        }

        public Interiors GetInteriorById(int Id)
        {
            if (_interiors.TryGetValue(Id, out Interiors interior))
                return interior;

            return null;
        }
    }
}
