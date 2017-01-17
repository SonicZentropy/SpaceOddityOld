// /** 
//  * ComponentECS.cs
//  * Dylan Bailey
//  * 20161210
// */

namespace Zenobit.Common.ZenECS
{
    #region Dependencies

    using System;
    using FullSerializer;
    using UnityEngine;
    using Zenobit.Common.Extensions;
    using Zenobit.Common.Serialization;

    #endregion

    public abstract class ComponentEcs : IJsonSerializable
    {
        private Type _objectType;

        public virtual string AssetName { get; }
        public virtual string AssetParentFolder { get; }
        public abstract ComponentTypes ComponentType { get; }
        public Guid Id { get; private set; }

        public Type ObjectType => _objectType ?? (_objectType = GetType());

        [HideInInspector]
        [fsIgnore]
        public Entity Owner { get; set; }

        /// NOTE child classes are responsible for manually subscribing to reactive properties and calling TriggerComponentUpdated
        public event Action<ComponentEcs> ComponentUpdated;

        protected void TriggerComponentUpdated<T>(T comp) where T : ComponentEcs
        {
            ComponentUpdated?.Invoke(comp);
        }

		public T GetComponent<T>() where T : ComponentEcs => Owner.GetComponent<T>();

		public virtual void Initialise(EcsEngine _engine, Entity owner)
        {
			SetOwner(owner);
			if (Id == Guid.Empty) SetId(Guid.NewGuid());
			
		}

        //[Inspect("IsNotOverridden", 500)]
        //[Method]
        public virtual void Save()
        {
            JsonSerializer.SaveToJson(this);
        }

        //[Inspect("IsNotOverridden", 501)]
        //[Method]
        public virtual void Load()
        {
            Clone();
        }

        public virtual bool IsNotOverridden()
        {
            return true;
        }

        public void Clone(ComponentEcs jsonToCloneFrom = null)
        {
            //First we create an instance of this specific type.

            if (jsonToCloneFrom == null)
                jsonToCloneFrom = JsonSerializer.LoadFromJson(this);
            ReflectionMethods.Clone(this, jsonToCloneFrom);
        }

        public void SetId(Guid id)
        {
            Id = id;
        }

        public virtual void OnDestroy()
        {
        }

        /// <summary>
        ///     Used by EntityFactory to set owner after entity creation. Can be overridden
        ///     by derived components who have child components that also need proper owner init.
        ///     See AvailableWeaponsComp as an example of this.
        /// </summary>
        /// <param name="entity">Entity to set as owner</param>
        public virtual void SetOwner(Entity entity)
        {
            Owner = entity;
        }

	    public ComponentEcs()
	    {
		    Id = Guid.NewGuid();
	    }
    }
}