// /**
//  * Entity.cs
//  * Dylan Bailey
//  * 20161210
// */


namespace Zenobit.Common.ZenECS
{
    #region Dependencies

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AdvancedInspector;
    using UnityEngine;

	#endregion

	[Serializable]
	public class Entity
    {
        //[ShowInInspector]

	    [HideInInspector]public Dictionary<Type, ComponentEcs> _components = new Dictionary<Type, ComponentEcs>();

	    [Inspect]public List<ComponentEcs> ComponentsList
	    {
		    get { return _components.Select(x => x.Value).ToList(); }
	    }

        public Entity()
        {
            //debug purposes
            //_components = new Dictionary<Type, ComponentEcs>();
        }

        public Entity(string entityName)
        {
            //debug purposes
            //_components = new Dictionary<Type, ComponentEcs>();
            EntityName = entityName;
        }

	    [Inspect]public string EntityName { get; set; } = "DefaultName";

	    [HideInInspector]public EntityWrapper Wrapper { get; set; }

	    [HideInInspector]public IEnumerable<ComponentEcs> Components => _components.Values;

		private EcsEngine _engine;

		public EcsEngine engine
		{
			set { _engine = value; }
			get
			{
				if (_engine == null)
					_engine = EcsEngine.Instance;
				return _engine;
			}
		}

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
		    //ZenLogger.Log($"GetComponentDownward failed!");
		    return null;
	    }

        public void AddComponent(ComponentTypes componentType)
        {
            var comp = ComponentFactory.Create(componentType);
	        //var comp = ComponentCache.Instance.Get(componentType);
            AddComponent(comp);
        }

        public T AddComponent<T>(ComponentTypes componentType) where T : ComponentEcs
        {
            var comp = ComponentFactory.Create(componentType);
	        //var comp = ComponentCache.Instance.Get(componentType);
	        AddComponent(comp);
            return (T) comp;
        }


        public void AddComponent(ComponentEcs component)
        {
			component.Initialise(engine, this);

			_components.Add(component.ObjectType, component);
	        engine?.AddComponent(component);
        }

	    public void RemoveComponent(ComponentEcs component)
	    {
		    _components.Remove(component.ObjectType);
			engine.DestroyComponent(component);
	    }
    }
}