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
        PLAYER,
        TEAM_PLAYER
    }

    class Pools
    {

        public static Pools Instance { get; } = new Pools();

        private readonly Dictionary<int, PXPlayer> playerPool = new Dictionary<int, PXPlayer>();
        private readonly Dictionary<int, List<PXPlayer>> teamPlayerPool = new Dictionary<int, List<PXPlayer>>();

        public Pools()
        {

        }

        public void Register(int Id, Entity entity)
        {
            switch (entity.Type)
            {
                case BaseObjectType.Player:
                    if (entity is IPlayer || entity is PXPlayer)
                    {
                        PXPlayer player = (PXPlayer)entity;

                        try
                        {
                            Instance.teamPlayerPool[player.Team.Id].Add(player);
                        }
                        catch (KeyNotFoundException)
                        {
                            Instance.teamPlayerPool[player.Team.Id] = new List<PXPlayer>
                            {
                                player
                            };
                        }

                        playerPool.Add(Id, player);
                    }
                    break;
            }
        }

        public HashSet<T> Get<T>(PoolType poolType, int poolId = 0) where T : IEntity
        {
            return poolType switch
            {
                PoolType.PLAYER => playerPool.Values.ToHashSet() as HashSet<T>,
                PoolType.TEAM_PLAYER => teamPlayerPool[poolId].ToHashSet() as HashSet<T>,
                _ => playerPool.Values.ToHashSet() as HashSet<T>,
            };
        }
    }
}
