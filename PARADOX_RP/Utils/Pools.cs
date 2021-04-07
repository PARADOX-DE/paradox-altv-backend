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
        VEHICLE,
        TEAM_PLAYER
    }

    class Pools
    {

        public static Pools Instance { get; } = new Pools();

        private readonly Dictionary<int, PXPlayer> playerPool = new Dictionary<int, PXPlayer>();
        private readonly Dictionary<int, PXVehicle> vehiclePool = new Dictionary<int, PXVehicle>();
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

                case BaseObjectType.Vehicle:
                    if (entity is IVehicle || entity is PXVehicle)
                    {
                        PXVehicle vehicle = (PXVehicle)entity;
                        vehiclePool.Add(Id, vehicle);
                    }
                    break;
            }
        }

        public HashSet<T> Get<T>(PoolType poolType, int poolId = 0) where T : IEntity
        {
            return poolType switch
            {
                PoolType.PLAYER => playerPool.Values.ToHashSet() as HashSet<T>,
                PoolType.VEHICLE => vehiclePool.Values.ToHashSet() as HashSet<T>,
                PoolType.TEAM_PLAYER => teamPlayerPool[poolId].ToHashSet() as HashSet<T>,
                _ => playerPool.Values.ToHashSet() as HashSet<T>,
            };
        }

        public bool Find<T>(PoolType poolType, int objId) where T : IEntity
        {
            Dictionary<int, T> targetDictionary = poolType switch
            {
                PoolType.PLAYER => playerPool as Dictionary<int, T>,
                PoolType.VEHICLE => vehiclePool as Dictionary<int, T>,
                PoolType.TEAM_PLAYER => teamPlayerPool as Dictionary<int, T>,
                _ => playerPool as Dictionary<int, T>,
            };

            if (targetDictionary.TryGetValue(objId, out _))
                return true;

            return false;
        }
    }
}
