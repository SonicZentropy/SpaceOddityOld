﻿// /** 
//  * EntityWrapper.cs
//  * Dylan Bailey
//  * 20161210
// */

namespace Zen.Common.ZenECS
{
    #region Dependencies

    using System.Linq;
    using AdvancedInspector;
    using FullInspector;
    using UnityEngine;
    using Zen.Common.ObjectPool;
    using Zen.Components;

    #endregion

    /// Allows viewing in Inspector for debug/dev purposes, serves no in-game function
    [AdvancedInspector(false, false)]
    public class EntityWrapper : BaseBehavior
    {
        //[fiInspectorOnly]
		//[Inspect]
		public Entity Entity;


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
                Debug.LogWarning($"Attempting to destroy an entity ({Entity.EntityName}) while ECS Engine is null.");
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
            //Debug.Log($"OnCollEnter");
            if (Entity.HasComponent(ComponentTypes.CollisionEnterComp))
                Entity.GetComponent<CollisionEnterComp>().Other.Add(other);
        }

        public void OnCollisionExit(Collision other)
        {
            //Debug.Log($"OnCollExit");
            if (Entity.HasComponent(ComponentTypes.CollisionExitComp))
                Entity.GetComponent<CollisionExitComp>().Other.Add(other);
        }

        public void OnTriggerEnter(Collider other)
        {
            //Debug.Log($"OnTriggerEnter");
            if (Entity.HasComponent(ComponentTypes.TriggerEnterComp))
                Entity.GetComponent<TriggerEnterComp>().Other.Add(other);
        }

        public void OnTriggerExit(Collider other)
        {
            //Debug.Log($"OnTriggerExit");
            if (Entity.HasComponent(ComponentTypes.TriggerExitComp))
                Entity.GetComponent<TriggerExitComp>().Other.Add(other);
        }

        private void InjectTransform()
        {
            Entity.GetComponent<PositionComp>().transform = transform;
        }
    }
}