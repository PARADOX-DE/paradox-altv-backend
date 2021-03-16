using AltV.Net.Elements.Entities;
using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PARADOX_RP.Utils
{
    enum PoolType
    {
        PLAYER
    }

    class Pools
    {
        
        public static Pools Instance { get; } = new Pools();

        private readonly Dictionary<int, Entity> playerPool = new Dictionary<int, Entity>();

        public Pools()
        {

        }

        public void Register(int Id, Entity entity)
        {
            switch (entity.Type)
            {
                case BaseObjectType.Player:
                    playerPool.Add(Id, entity);
                    break;
            }
        }

        public HashSet<Entity> Get(PoolType poolType)
        {
            return poolType switch
            {
                PoolType.PLAYER => playerPool.Values.ToHashSet(),
                _ => playerPool.Values.ToHashSet(),
            };
        }
    }
}
