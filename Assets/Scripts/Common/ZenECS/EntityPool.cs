// // /**
// //  * EntityPool.cs
// //  * Dylan Bailey
// //  * 20170213
// // */
namespace Zen.Common.ZenECS
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Zen.Common.Extensions;
    using Zen.Components;
    using Zen.Editor.Utils;

    public class EntityPool : Singleton<EntityPool>
    {
        public Dictionary<string, Stack<Entity>> EntityMap = new Dictionary<string, Stack<Entity>>();

        public bool CheckPoolExists(string entity)
        {
            if (entity == null) return false;
            return EntityMap.ContainsKey(FileOps.GetEntityNameFromFullName(entity));
        }

        public bool CheckPoolHasFreeItems(string entity)
        {
            if (!CheckPoolExists(entity)) return false;

            return EntityMap[entity].Count > 0;
        }

        public Entity RetrieveFromPool(string entity)
        {
            if (entity != null && EntityMap.ContainsKey(entity))
            {
                var stack = EntityMap[entity];
                if (stack.Count > 0)
                {
                    Entity e = stack.Pop();
                    return e;
                }
            }
            return null;
        }

        public void ReleaseToPool(Entity e)
        {
            e.Enabled = false;
            CleanupEntity(e);
            if (!EntityMap.ContainsKey(e.EntityName))  // Make new entity pool for this entity type
            {
                var stack = new Stack<Entity>();
                stack.Push(e);
                EntityMap[e.EntityName] = stack;
            }
            else
            {
                EntityMap[e.EntityName].Push(e);
            }
        }

        private void CleanupEntity(Entity e)
        {
            //e.RemoveComponent<DamageComp>();
        }

        public void ResetEntityPool()
        {
            EntityMap.Clear();
        }
    }
}