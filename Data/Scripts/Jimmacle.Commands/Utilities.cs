namespace Jimmacle.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Sandbox;
    using Sandbox.Common;
    using Sandbox.Definitions;
    using Sandbox.Game;
    using Sandbox.ModAPI;

    using VRage;
    using VRageMath;
    using VRage.ModAPI;

    public class Utilities
    {
        public static IMyCubeGrid GetGrid(string displayName)
        {
            HashSet<IMyEntity> entities = new HashSet<IMyEntity>();
            MyAPIGateway.Entities.GetEntities(entities, e => e is IMyCubeGrid);
            foreach (var entity in entities)
            {
                if (entity.DisplayName == displayName)
                {
                    return entity as IMyCubeGrid;
                }
            }
            throw new Exception("Grid not found");
        }

        public static IMyCubeGrid GetGrid(long entityId)
        {
            HashSet<IMyEntity> entities = new HashSet<IMyEntity>();
            MyAPIGateway.Entities.GetEntities(entities, e => e is IMyCubeGrid);
            foreach (var entity in entities)
            {
                if (entity.EntityId == entityId)
                {
                    return entity as IMyCubeGrid;
                }
            }
            throw new Exception("Grid not found");
        }

        public static IMyIdentity GetIdentity(string name)
        {
            List<IMyIdentity> identities = new List<IMyIdentity>();
            MyAPIGateway.Players.GetAllIdentites(identities);
            foreach (var identity in identities)
            {
                if (identity.DisplayName == name)
                {
                    return identity;
                }
            }
            throw new Exception("Identity not found");
        }

        public static IMyIdentity GetIdentity(long id)
        {
            List<IMyIdentity> identities = new List<IMyIdentity>();
            MyAPIGateway.Players.GetAllIdentites(identities);
            foreach (var identity in identities)
            {
                if (identity.IdentityId == id)
                {
                    return identity;
                }
            }
            throw new Exception("Identity not found");
        }
    }
}
