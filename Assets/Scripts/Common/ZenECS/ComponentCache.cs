using System.Collections.Generic;
using Zen.Common.Extensions;
using Zen.Common.ZenECS;

public class ComponentCache : Singleton<ComponentCache>
{
	public Dictionary<ComponentTypes, ComponentPool> _componentPools= new Dictionary<ComponentTypes, ComponentPool>();

	public ComponentCache()
	{
		//_componentPools  = new Dictionary<ComponentTypes, ComponentPool>();
	}

	public ComponentPool GetComponentPool(ComponentTypes ct)
	{
		ComponentPool componentPool;

		if(!_componentPools.TryGetValue(ct, out componentPool))
		{
			componentPool = CreateNewComponentPool(ct);
		}

		return componentPool;
	}

	private ComponentPool CreateNewComponentPool(ComponentTypes ct)
	{
		if (!_componentPools.ContainsKey(ct))
		{
			var _componentPool = new ComponentPool();
			_componentPools.Add(ct, _componentPool);
			return _componentPool;
		}
		return _componentPools[ct];
	}

	public ComponentEcs Get(ComponentTypes ct)
	{
		return GetComponentPool(ct).Get(ct);
	}

	public void Release(ComponentEcs obj)
	{
		if (!_componentPools.ContainsKey(obj.ComponentType))
			CreateNewComponentPool(obj.ComponentType);
		_componentPools[obj.ComponentType].Push(obj);
	}

	//public void RegisterCustomComponentPool<T>(ComponentPool<T> componentPool) where T : ComponentEcs
	//{
	//	_componentPools.Add(typeof(T), componentPool);
	//}

	public void Reset()
	{
		_componentPools.Clear();
	}
}


public class ComponentPool
{
	//Func<T> _factoryMethod;
	//Action<T> _resetMethod;
	Stack<ComponentEcs> _pool;

	public ComponentPool()
	{
		_pool = new Stack<ComponentEcs>();
	}

	public ComponentEcs Get(ComponentTypes ct)
	{
		return _pool.Count == 0
			       ? ComponentFactory.Instantiate(ct)
			       : _pool.Pop();
	}

	public void Push(ComponentEcs obj)
	{
		_pool.Push(obj);
	}
}