﻿// /** 
//  * Entity.cs
//  * Dylan Bailey
//  * 20161210
// */

namespace Zenobit.Common.ZenECS
{
    #region Dependencies

    using System;
    using System.Collections.Generic;

    #endregion

    public class Entity
    {
        //[ShowInInspector]
        public Dictionary<Type, ComponentEcs> _components; // = new Dictionary<Type, ComponentECS>();

        public Entity()
        {
            //debug purposes
            _components = new Dictionary<Type, ComponentEcs>();
        }

        public Entity(string entityName)
        {
            //debug purposes
            _components = new Dictionary<Type, ComponentEcs>();
            EntityName = entityName;
        }

        public string EntityName { get; set; }

        public EntityWrapper Wrapper { get; set; }

        public IEnumerable<ComponentEcs> Components => _components.Values;

        public void InitializeComponents(List<ComponentTypes> componentTypes)
        {
            foreach (var component in componentTypes)
            {
                AddComponent(component);
            }
        }

        public bool HasComponent(ComponentTypes type)
        {
            return ComponentFactory.ComponentLookup.ContainsKey(type) &&
                   _components.ContainsKey(ComponentFactory.ComponentLookup[type]);
        }

        public T GetComponent<T>() where T : ComponentEcs => (T) _components[typeof(T)];

	    public T GetComponentDownward<T>() where T : ComponentEcs
	    {
		    if (_components.ContainsKey(typeof(T)))
		    {
			    return GetComponent<T>();
		    }
		    else
		    {
			    var wrappers = Wrapper.GetComponentsInChildren<EntityWrapper>();
			    foreach (EntityWrapper awrapper in wrappers)
			    {
				    if (awrapper == this.Wrapper)
					    continue;
				    var ent = awrapper.Entity;
					if (ent._components.ContainsKey(typeof(T)))
					{
						return ent.GetComponent<T>();
					}
				}
		    }
		    ZenLogger.Log($"GetComponentDownward failed!");
		    return null;
	    }

        public void AddComponent(ComponentTypes componentType)
        {
            var comp = ComponentFactory.Create(componentType);
            AddComponent(comp);
        }

        public T AddComponent<T>(ComponentTypes componentType) where T : ComponentEcs
        {
            var comp = ComponentFactory.Create(componentType);
            AddComponent(comp);
            return (T) comp;
        }


        public void AddComponent(ComponentEcs component)
        {
            _components.Add(component.ObjectType, component);
            if (EcsEngine.Instance != null)
                EcsEngine.Instance.AddComponent(component);
        }
    }
}