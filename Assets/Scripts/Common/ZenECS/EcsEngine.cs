// /** 
//  * EcsEngine.cs
//  * Will Hart and Dylan Bailey
//  * 20161205
// */

using Zenobit.Common.Debug;

namespace Zenobit.Common.ZenECS
{
    #region Dependencies

    using Components;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using AdvancedInspector;
    using Common.ObjectPool;
    using Editor.Utils;
    using Extensions;
    using Zenobit.Systems;
    using UnityEngine;

    #endregion

    public class EcsEngine 
	{
		private readonly Dictionary<Guid, ComponentEcs> _componentsById = new Dictionary<Guid, ComponentEcs>();

	    public List<Entity> EntityList { get; set; } = new List<Entity>();

	    [Descriptor("Comps:", "Components On Entity")]
		[SerializeField] private readonly Dictionary<ComponentTypes, List<ComponentEcs>> _componentPools =
			new Dictionary<ComponentTypes, List<ComponentEcs>>(Enum.GetNames(typeof(ComponentTypes)).Length, new FastEnumIntEqualityComparer<ComponentTypes>());

		[SerializeField] private readonly List<IEcsSystem> _systemPool = new List<IEcsSystem>();

		public readonly EntityFactory Factory;

		public Guid CurrentHash = Guid.NewGuid();

		public event Action<ComponentEcs> OnComponentAdded;
		public event Action<ComponentEcs> OnComponentRemoved;
		public event Action<Entity> OnEntityAdded;
		public event Action<Entity> OnEntityRemoved;

		//ZenBehaviour stuff
		private List<IOnStart> OnStartList = new List<IOnStart>();
		private List<IOnStart> StartToRemoveList = new List<IOnStart>();
		private List<IOnUpdate> OnUpdateList = new List<IOnUpdate>();
		private List<IOnFixedUpdate> OnFixedUpdateList = new List<IOnFixedUpdate>();
		private List<IOnLateUpdate> OnLateUpdateList = new List<IOnLateUpdate>();
		private List<IInitAfterECS> InitAfterEcsList = new List<IInitAfterECS>();
		private Dictionary<Type, ZenImplementationFlags> typeImplDict = new Dictionary<Type, ZenImplementationFlags>();

		/// <summary>
		/// Add a component to the pool
		/// </summary>
		/// <param name="component"></param>
		public void AddComponent(ComponentEcs component)
		{
			if (Application.isEditor && !Application.isPlaying)
			{
				return;
			}
			if (!_componentPools.ContainsKey(component.ComponentType))
			{
				ZenLogger.LogWarning($"Attempted to add unknown component type to Component Pools - {component.ComponentType}");
			}
			else
			{
				_componentPools[component.ComponentType].Add(component);
			}

			if (_componentsById.ContainsKey(component.Id))
			{
				if (!Application.isEditor || Application.isPlaying)
				{
					ZenLogger.LogError("Attempted to add existing GUID");
				}
			}
			else
			{
				_componentsById.Add(component.Id, component);
			}
			CurrentHash = Guid.NewGuid();

			OnComponentAdded?.Invoke(component);
		}

		public T GetById<T>(Guid id) where T : ComponentEcs
		{
			if (!_componentsById.ContainsKey(id)) return null;
			return _componentsById[id] as T;
		}

		public List<ComponentEcs> Get(ComponentTypes type)
		{
			return _componentPools[type];
		}

		public IEnumerable<T> Get<T>(ComponentTypes type) where T : ComponentEcs
		{
			return _componentPools[type].OfType<T>();
		}

		public ComponentEcs GetSingle(ComponentTypes type)
		{
			return _componentPools[type][0];
		}

		public T GetSingle<T>(ComponentTypes type) where T : ComponentEcs
		{
			return _componentPools[type].OfType<T>().First();
		}

		public void Reset()
		{
			foreach (var componentList in _componentPools.Values)
			{
				var tempList = new List<ComponentEcs>(componentList);
				foreach (var component in tempList)
				{
					DestroyComponent(component);
				}
			}

			_componentPools.Clear();
			_componentsById.Clear();
			EntityList.Clear();

			var enumVals = Enum.GetValues(typeof(ComponentTypes));

			foreach (var ev in enumVals)
			{
				var ct = (ComponentTypes) ev;
				_componentPools.Add(ct, new List<ComponentEcs>());
			}

			CurrentHash = Guid.NewGuid();
		}

		public Entity CreateEntity(string entityName)
		{
			return Factory.CreateEntityFromTemplate(entityName);
		}

		public void DestroyEntity(Entity entity)
        {
            OnEntityRemoved?.Invoke(entity);

            foreach (var comp in entity.Components)
            {
                var upf = comp as UnityPrefabComp;
                if (upf != null)
                {
                    var go = upf.Owner.Wrapper.gameObject;
                    if (upf.IsPooled)
                    {
                        go.Release();
                    }
                    else
                    {
                        UnityEngine.Object.Destroy(go);
                    }
                }

                DestroyComponent(comp);
            }
        }

		/// <summary>
		/// Finds an entity from Res.Entity mapping - DO NOT USE IN UPDATE!
		/// </summary>
		/// <param name="entityToFind">Res.Entity string mapping</param>
		/// <returns>The Entity which matches or null</returns>
	    public Entity FindEntity(string entityToFind)
		{
			string entName = FileOps.GetStringAfterLastSlash(entityToFind);

			return EntityList.Find(x => x.EntityName.Equals(entName));
	    }

		public void DestroyComponent(ComponentEcs component)
		{
			component.OnDestroy();
			OnComponentRemoved?.Invoke(component);
			_componentPools[component.ComponentType].Remove(component);
			_componentsById.Remove(component.Id);

			CurrentHash = Guid.NewGuid();
		}

		public EcsEngine AddSystem(IEcsSystem system)
		{
			if (system.Init())
			{
				_systemPool.Add(system);
			}

			return this;
		}

		public void Update()
		{
			ProcessAwaitingStart();
			for (int i = 0; i < OnUpdateList.Count; i++)
			{
				var idx = OnUpdateList[i];
				if (idx.IsActive)
				{
					OnUpdateList[i].OnUpdate();
				}
			}

			foreach (var system in _systemPool)
			{
				system.Update();
			}
		}

		public void FixedUpdate()
		{
			ProcessAwaitingStart();
			for (int i = 0; i < OnFixedUpdateList.Count; i++)
			{
				var idx = OnFixedUpdateList[i];
				if (idx.IsActive)
				{
					idx.OnFixedUpdate();
				}
			}

			foreach (var system in _systemPool)
			{
				system.FixedUpdate();
			}
		}

		public void LateUpdate()
		{
			for (int i = 0; i < OnLateUpdateList.Count; i++)
			{
				var idx = OnLateUpdateList[i];
				if (idx.IsActive)
				{
					idx.OnLateUpdate();
				}
			}

			foreach (var system in _systemPool)
			{
				system.LateUpdate();
			}
		}

        public void TriggerEntityAdded(Entity entity)
        {
            OnEntityAdded?.Invoke(entity);
        }

        public void TriggerEntityRemoved(Entity entity)
        {
            OnEntityRemoved?.Invoke(entity);
        }

		#region ZenBehaviours
		public void RegisterZenBehaviour(IZenBehaviour newZenBehaviour)
		{
			if (Application.isEditor && !Application.isPlaying)
			{
				return;
			}
			ZenImplementationFlags currFlags;
			if (typeImplDict.ContainsKey(newZenBehaviour.ObjectType))
			{
				//already loaded this type, add to lists via impl flags
				typeImplDict.TryGetValue(newZenBehaviour.ObjectType, out currFlags);
			}
			else
			{
				//First encounter with this type
				currFlags.HasOnAwake = newZenBehaviour is IOnAwake;
				currFlags.HasOnStart = newZenBehaviour is IOnStart;
				currFlags.HasOnUpdate = newZenBehaviour is IOnUpdate;
				currFlags.HasOnFixedUpdate = newZenBehaviour is IOnFixedUpdate;
				currFlags.HasOnLateUpdate = newZenBehaviour is IOnLateUpdate;
				currFlags.HasInitAfterECS = newZenBehaviour is IInitAfterECS;
				typeImplDict.Add(newZenBehaviour.ObjectType, currFlags);
			}

			if (currFlags.HasOnStart)
			{
				OnStartList.Add((IOnStart)newZenBehaviour);
				OnStartList.Sort((a, b) => a.ExecutionPriority.CompareTo(b.ExecutionPriority));
			}
			if (currFlags.HasOnUpdate)
			{
				OnUpdateList.Add((IOnUpdate)newZenBehaviour);
				OnUpdateList.Sort((a, b) => a.ExecutionPriority.CompareTo(b.ExecutionPriority));
			}
			if (currFlags.HasOnFixedUpdate)
			{
				OnFixedUpdateList.Add((IOnFixedUpdate)newZenBehaviour);
				OnFixedUpdateList.Sort((a, b) => a.ExecutionPriority.CompareTo(b.ExecutionPriority));
			}
			if (currFlags.HasOnLateUpdate)
			{
				OnLateUpdateList.Add((IOnLateUpdate)newZenBehaviour);
				OnLateUpdateList.Sort((a, b) => a.ExecutionPriority.CompareTo(b.ExecutionPriority));
			}
			if (currFlags.HasInitAfterECS)
			{
				InitAfterEcsList.Add((IInitAfterECS) newZenBehaviour);
				InitAfterEcsList.Sort((a, b) => a.ExecutionPriority.CompareTo(b.ExecutionPriority));
			}

			if (currFlags.HasOnAwake) ((IOnAwake)newZenBehaviour).OnAwake();
		}

		public void DeregisterZenBehaviour(IZenBehaviour removedZenBehaviour)
		{
			ZenImplementationFlags currFlags;

			//already loaded this type, add to lists via impl flags
			typeImplDict.TryGetValue(removedZenBehaviour.ObjectType, out currFlags);

			if (currFlags.HasOnStart)
			{
				OnStartList.Remove((IOnStart)removedZenBehaviour);
			}
			if (currFlags.HasOnUpdate)
			{
				OnUpdateList.Remove((IOnUpdate)removedZenBehaviour);
			}
			if (currFlags.HasOnFixedUpdate)
			{
				OnFixedUpdateList.Remove((IOnFixedUpdate)removedZenBehaviour);
			}
			if (currFlags.HasOnLateUpdate)
			{
				OnLateUpdateList.Remove((IOnLateUpdate)removedZenBehaviour);
			}
			if (currFlags.HasInitAfterECS)
			{
				InitAfterEcsList.Remove((IInitAfterECS) removedZenBehaviour);
			}
		}

	    public void InitAfterECS()
	    {
		    for (int i = 0; i < InitAfterEcsList.Count; i++)
		    {
			    InitAfterEcsList[i].InitAfterECS();
		    }
	    }

		void ProcessAwaitingStart()
		{
			if (OnStartList.Count <= 0) return;

			StartToRemoveList.Clear();

			for (int i = 0; i < OnStartList.Count; i++)
			{
				StartToRemoveList.Add(OnStartList[i]);
			}

			for (int i = 0; i < StartToRemoveList.Count; i++)
			{
				var idx = StartToRemoveList[i];
				if (idx.IsActive)
				{
					idx.OnStart();
					//Debug.Log($"Removing: {idx.GetType().Name}");
					OnStartList.Remove(idx);
				}
			}

		}
		#endregion

		#region Singleton Implementation

		private EcsEngine()
		{
			Factory = new EntityFactory();
		}

		private static EcsEngine _instance;

		public static EcsEngine Instance
		{
			get
			{
				if (_instance != null) return _instance;

				_instance = new EcsEngine();
				_instance.Reset();
				return _instance;
			}
			private set { _instance = value; }
		}

		#endregion
	}

	// todo; check if your TEnum is enum && typeCode == TypeCode.Int
	struct FastEnumIntEqualityComparer<TEnum> : IEqualityComparer<TEnum>
		where TEnum : struct
	{
		static class BoxAvoidance
		{
			static readonly Func<TEnum, int> _wrapper;

			public static int ToInt(TEnum enu)
			{
				return _wrapper(enu);
			}

			static BoxAvoidance()
			{
				var p = Expression.Parameter(typeof(TEnum), null);
				var c = Expression.ConvertChecked(p, typeof(int));

				_wrapper = Expression.Lambda<Func<TEnum, int>>(c, p).Compile();
			}
		}

		public bool Equals(TEnum firstEnum, TEnum secondEnum)
		{
			return BoxAvoidance.ToInt(firstEnum) ==
				BoxAvoidance.ToInt(secondEnum);
		}

		public int GetHashCode(TEnum firstEnum)
		{
			return BoxAvoidance.ToInt(firstEnum);
		}
	}

	public struct ZenImplementationFlags
	{
		public bool HasOnAwake;
		public bool HasOnStart;
		public bool HasOnUpdate;
		public bool HasOnFixedUpdate;
		public bool HasOnLateUpdate;
		public bool HasInitAfterECS;
	}

}