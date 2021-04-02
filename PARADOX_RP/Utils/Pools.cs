using AltV.Net;
using AltV.Net.Async;
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

        private readonly Dictionary<int, PXPlayer> playerPool = new Dictionary<int, PXPlayer>();

        public Pools()
        {

        }

        public void Register(int Id, Entity entity)
        {
            switch (entity.Type)
            {
                case BaseObjectType.Player:
                    if (entity is IPlayer || entity is PXPlayer)
                        playerPool.Add(Id, (PXPlayer)entity);
                    break;
            }
        }

        public HashSet<T> Get<T>(PoolType poolType) where T : IEntity
        {
            return poolType switch
            {
                PoolType.PLAYER => playerPool.Values.ToHashSet() as HashSet<T>,
                _ => playerPool.Values.ToHashSet() as HashSet<T>,
            };
        }
    }
}
