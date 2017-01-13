// /** 
//  * EntityWrapper.cs
//  * Dylan Bailey
//  * 20161210
// */

namespace Zenobit.Common.ZenECS
{
    #region Dependencies

    using System.Linq;
    using FullInspector;
    using Serialization;
    using UnityEngine;
    using Zenobit.Common.Debug;
    using Zenobit.Common.ObjectPool;
    using Zenobit.Components;

    #endregion

    /// Allows viewing in Inspector for debug/dev purposes, serves no in-game function
    public class EntityWrapper : BaseBehavior //BaseBehavior 
    {
        [fiInspectorOnly] public Entity Entity;


        public void SetEntity(Entity entity)
        {
            Entity = entity;
            InjectTransform();
            Entity.Wrapper = this;
        }

        private void OnDestroy()
        {
            var isPooled = Entity.HasComponent(ComponentTypes.UnityPrefabComp) &&
                           Entity.GetComponent<UnityPrefabComp>().IsPooled;

            if (EcsEngine.Instance == null)
            {
                ZenLogger.LogWarning($"Attempting to destroy an entity ({Entity.EntityName}) while ECS Engine is null.");
            }
            else
            {
                foreach (var comp in Entity.Components.Where(c => c != null))
                {
                    EcsEngine.Instance.DestroyComponent(comp);
                }
            }

            if (isPooled)
            {
                gameObject.Release();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void OnCollisionEnter(Collision other)
        {
            //ZenLogger.Log($"OnCollEnter");
            if (Entity.HasComponent(ComponentTypes.CollisionEnterComp))
                Entity.GetComponent<CollisionEnterComp>().Other.Add(other);
        }

        public void OnCollisionExit(Collision other)
        {
            //ZenLogger.Log($"OnCollExit");
            if (Entity.HasComponent(ComponentTypes.CollisionExitComp))
                Entity.GetComponent<CollisionExitComp>().Other.Add(other);
        }

        public void OnTriggerEnter(Collider other)
        {
            //ZenLogger.Log($"OnTriggerEnter");
            if (Entity.HasComponent(ComponentTypes.TriggerEnterComp))
                Entity.GetComponent<TriggerEnterComp>().Other.Add(other);
        }

        public void OnTriggerExit(Collider other)
        {
            //ZenLogger.Log($"OnTriggerExit");
            if (Entity.HasComponent(ComponentTypes.TriggerExitComp))
                Entity.GetComponent<TriggerExitComp>().Other.Add(other);
        }

        private void InjectTransform()
        {
            Entity.GetComponent<PositionComp>().transform = transform;
        }
    }
}