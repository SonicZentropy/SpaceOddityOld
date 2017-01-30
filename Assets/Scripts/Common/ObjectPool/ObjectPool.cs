// /**
//  * ObjectPool.cs
//  * Dylan Bailey
//  * 20161209
// */

#pragma warning disable 0414, 0219, 649, 169, 618, 1570

namespace Zen.Common.ObjectPool
{

    #region Dependencies

    using System;
    using System.Collections.Generic;
    using AdvancedInspector;
    using MEC;
    using UnityEngine;
    using Object = UnityEngine.Object;

    #endregion

    public static class ObjectPoolExtensions
    {
        public static GameObject InstantiateFromPool(this GameObject prefab)
        {
            return ObjectPool.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }

		public static GameObject InstantiateFromPool(this GameObject prefab, Vector3 position)
		{
			return ObjectPool.Instantiate(prefab, position, Quaternion.identity);
		}

		public static GameObject InstantiateFromPool(this GameObject prefab, Vector3 position, Quaternion rotation)
		{
			return ObjectPool.Instantiate(prefab, position, rotation);
		}

		public static T InstantiateFromPool<T>(this GameObject prefab)
            where T : class
        {
            var tComp = ObjectPool.Instantiate(prefab).GetComponent<T>();
            if (tComp == null) ZenLogger.LogError("Object of type " + typeof(T).Name + " is not contained in prefab");
            return tComp;
        }

        public static T InstantiateFromPool<T>(this GameObject prefab, Vector3 position, Quaternion rotation)
            where T : class
        {
            var tComp = ObjectPool.Instantiate(prefab, position, rotation).GetComponent<T>();
            if (tComp == null) ZenLogger.LogError("Object of type " + typeof(T).Name + " is not contained in prefab");
            return tComp;
        }

        public static GameObject InstantiateFromPool(
            this GameObject prefab,
            Vector3 position,
            Quaternion rotation,
            GameObject activeParent,
            GameObject inactiveParent)
        {
            return ObjectPool.Instantiate(prefab, position, rotation, activeParent, inactiveParent);
        }

        public static T InstantiateFromPool<T>(
            this GameObject prefab,
            Vector3 position,
            Quaternion rotation,
            GameObject activeParent,
            GameObject inactiveParent)
            where T : class
        {
            return ObjectPool.Instantiate(prefab, position, rotation, activeParent, inactiveParent) as T;
        }

        public static void Release(this GameObject objthis)
        {
            ObjectPool.Release(objthis);
        }

        //public static void Release<T>(this T objThis) where T: UnityEngine.Component
        //{
        //	ObjectPool.Release(objThis.gameObject);
        //}

        public static void ReleaseDelayed(this GameObject objthis, float delayTime)
        {
            ObjectPool.DelayedRelease(objthis, delayTime);
        }
    }

    public class ObjectPool : MonoBehaviour
    {
        private static ObjectPool _instance;
        public GameObject ActiveParentDefault;
        public GameObject InactiveParentDefault;

        public List<Pool> CustomPools = new List<Pool>();

        readonly Dictionary<GameObject, Pool> _pool = new Dictionary<GameObject, Pool>();

        public List<Pool> RuntimePools = new List<Pool>();
        //Read only, just displays pools created on runtime without prior setup."

	    [HideInInspector]public static bool isShuttingDown = false;

	    void OnApplicationQuit()
	    {
		    isShuttingDown = true;
	    }

        public static ObjectPool Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = FindObjectOfType<ObjectPool>();
                if (_instance != null) return _instance;

                var go = new GameObject("ObjectPool(Generated)");
                _instance = go.AddComponent<ObjectPool>();
                return _instance;
            }
        }

        public static GameObject Instantiate(
            GameObject prefab,
            Vector3 position,
            Quaternion rotation,
            GameObject activeParent = null,
            GameObject inactiveParent = null)

        {
	        if (isShuttingDown) return null;
            return Instance._Instantiate(prefab, position, rotation, activeParent, inactiveParent);
        }

        private GameObject _Instantiate(
            GameObject prefab,
            Vector3 position,
            Quaternion rotation,
            GameObject activeParent = null,
            GameObject inactiveParent = null)
        {
            if (_pool.ContainsKey(prefab))
            {
                var current = _pool[prefab];

                return current.Request(position, rotation);
            }
            var newpool = new Pool();


            //if (activeParent) newpool.activeParentGO = activeParent;
            //else newpool.activeParentGO = ActiveParentDefault;
            //if (inactiveParent) newpool.inactiveParentGO = inactiveParent;
            //else newpool.inactiveParentGO = InactiveParentDefault;

            var keywordActiveGameObject = new GameObject(prefab.name);
            keywordActiveGameObject.transform.SetParent(
                activeParent != null ? activeParent.transform : ActiveParentDefault.transform);
            newpool.ActiveParentGo = keywordActiveGameObject;

            var keywordInactiveGameObject = new GameObject(prefab.name);
            keywordInactiveGameObject.transform.SetParent(
                inactiveParent != null ? inactiveParent.transform : InactiveParentDefault.transform);
            newpool.InactiveParentGo = keywordInactiveGameObject;

            RuntimePools.Add(newpool);
            _pool.Add(prefab, newpool);
            newpool.MaxObjectsWarning = 1000;
            newpool.Prefab = prefab;
            return newpool.Request(position, rotation);
        }

        private void Awake()
        {
            if (ActiveParentDefault == null) ActiveParentDefault = GameObject.Find("Active");
            if (InactiveParentDefault == null) InactiveParentDefault = GameObject.Find("Inactive");

            foreach (var p in CustomPools)
            {
                if (p.Prefab == null)
                {
                    ZenLogger.LogError("Custom object pool exists without prefab assigned to it.");
                    continue;
                }
                //Set keyword if blank
                if (p.Keyword.Length == 0) p.Keyword = p.Prefab.name;
                //Set custom pool's parents to the default if they don't exist
                var addSortObject = false;
                if (p.ActiveParentGo == null)
                {
                    p.ActiveParentGo = ActiveParentDefault;
                    addSortObject = true;
                    var keywordActiveGameObject = new GameObject(p.Keyword);
                    keywordActiveGameObject.transform.SetParent(p.ActiveParentGo.transform);
                    p.ActiveParentGo = keywordActiveGameObject;
                }
                if (p.InactiveParentGo == null)
                {
                    p.InactiveParentGo = InactiveParentDefault;
                    addSortObject = true;
                    var keywordInactiveGameObject = new GameObject(p.Keyword);
                    keywordInactiveGameObject.transform.SetParent(p.InactiveParentGo.transform);
                    p.InactiveParentGo = keywordInactiveGameObject;
                }

                if (!_pool.ContainsKey(p.Prefab))
                {
                    _pool.Add(p.Prefab, p);
                }
                else
                {
                    ZenLogger.LogError("Trying to add object that is already registered by object pooler.");
                }
            }
        }

        private void Start()
        {
            foreach (var p in CustomPools)
            {
                p.PreloadInstances();
            }
        }

        public static void Release(GameObject obj)
        {
	        if (isShuttingDown)
	        {
		        Destroy(obj);
		        return;
	        }
			Instance._release(obj);
        }

        void _release(GameObject obj)
        {
            ObjectPoolId id = obj.GetComponent<ObjectPoolId>();
            if (id == null || id.Free)
            {
                return;
            }

            //Culling
            // Trigger culling if the feature is ON and the size  of the
            //   overall pool is over the Cull Above threashold.
            //   This is triggered here because Despawn has to occur before
            //   it is worth culling anyway, and it is run fairly often.
            if (!id.Pool.CullingActive && // Cheap & Singleton. Only trigger once!
                id.Pool.cullDespawned && // Is the feature even on? Cheap too.
                (id.Pool.CountTotal > id.Pool.CullAboveCount)) // Criteria met?
            {
                id.Pool.CullingActive = true;
                //StartCoroutine(id.Pool.CullDespawned());
                Timing.RunCoroutine(id.Pool.CullDespawned(), Segment.SlowUpdate);
            }

	        Timing.KillCoroutines(obj.name);

            id.Pool.Release(id);
        }

        public static void DelayedRelease(GameObject obj, float delayTime)
        {
			if (isShuttingDown)
			{
				Destroy(obj);
				return;
			}
			Instance._delayedRelease(obj, delayTime);
        }

        private void _delayedRelease(GameObject obj, float delayTime)
        {
			Timing.RunCoroutine(_delayedReleaseCoroutine(obj, delayTime), obj.name);
		}

        IEnumerator<float> _delayedReleaseCoroutine(GameObject obj, float delayTime)
        {
	        yield return Timing.WaitForSeconds(delayTime);
            _release(obj);
            yield return 0f;
        }

	    private void OnDestroy()
	    {
		    //foreach (var p in CustomPools)
		    //{
			//	p.DestroyAll();
			//}
			//
		    //foreach (var p in RuntimePools)
		    //{
			//    p.DestroyAll();
		    //}

		    var go = GameObject.Find("ObjectPool(Generated)");
		    if (go != null) Destroy(go);
	    }

        [Serializable]
        public class Pool //: MonoBehaviour
        {
            public GameObject Prefab;
            public string Keyword;
            public GameObject ActiveParentGo;
            public GameObject InactiveParentGo;
            public uint PreloadCount;
	        public bool NeedsCustomInit;
	        public bool NeedsCustomRelease;

            [ReadOnly] public int CountFree;
            [ReadOnly] public int CountInUse;

	        public bool cullDespawned = false;

			[Inspect("CullDespawn")]
			public uint CullAboveCount = 100;

			[Inspect("CullDespawn")]
			public int CullMaxPerPass = 1000;

			[Inspect("CullDespawn")]
			public float CullWaitDelay = 60;

			[Inspect("CullDespawn")]
			public int MaxObjectsWarning = 1000;

            [HideInInspector]public int CountTotal => CountFree + CountInUse;
            [HideInInspector]public bool CullingActive;
            private readonly List<ObjectPoolId> _free = new List<ObjectPoolId>();
            private readonly List<ObjectPoolId> _inUse = new List<ObjectPoolId>();
	        private bool CullDespawn => cullDespawned;

            private GameObject _temp;

            public override string ToString()
            {
                return Prefab == null ? "Empty Pool" : Prefab.ToString();
            }

            public void PreloadInstances()
            {
                if (Keyword.Length == 0) Keyword = Prefab.gameObject.name;
                if (PreloadCount <= 0)
                {
                    return;
                }

                for (var i = 0; i < PreloadCount; i++)
                {
                    _temp = Object.Instantiate(Prefab, Vector3.zero, Quaternion.identity);
                    _temp.name += CountTotal;
                    var obj = _temp.AddComponent<ObjectPoolId>();
                    _free.Add(obj);
                    CountFree = _free.Count;

                    obj.Pool = this;
                    obj.MyParentTransform = ActiveParentGo ? ActiveParentGo.transform : Prefab.transform.parent;

                    obj.transform.SetParent(obj.MyParentTransform);
                    if (CountTotal > MaxObjectsWarning)
                    {
                        //ZenLogger.LogError("ObjectPool: More than max objects spawned. --- " + prefab.name + " Max obj set to: " + MaxObjectsWarning + " and the pool already has: " + CountTotal);
                    }

                    obj.transform.SetParent(!InactiveParentGo ? Instance.transform : InactiveParentGo.transform);
                    obj.SetFree(true);
                    obj.gameObject.SetActive(false);
                }
            }

            public GameObject Request(Vector3 position, Quaternion rotation)
            {
                ObjectPoolId obj;
                if (CountFree <= 0)
                {
                    _temp = Object.Instantiate(Prefab, position, rotation);
                    _temp.name += CountTotal;
                    obj = _temp.AddComponent<ObjectPoolId>();
                    _inUse.Add(obj);
                    CountInUse = _inUse.Count;

                    obj.Pool = this;
                    obj.MyParentTransform = ActiveParentGo ? ActiveParentGo.transform : Prefab.transform.parent;

                    obj.transform.SetParent(obj.MyParentTransform);
                    //if (CountTotal > MaxObjectsWarning)
                    //{
                    //    //ZenLogger.LogError("ObjectPool: More than max objects spawned. --- " + prefab.name + " Max obj set to: " + MaxObjectsWarning + " and the pool already has: " + CountTotal);
                    //}
                    obj.SetFree(false);
                }
                else
                {
                    obj = _free[_free.Count - 1];

                    _free.RemoveAt(_free.Count - 1);

                    _inUse.Add(obj);
                    obj.transform.SetParent(obj.MyParentTransform);

                    obj.gameObject.transform.position = position;
                    obj.gameObject.transform.rotation = rotation;
                    //obj.gameObject.SetActive(true);

                    CountFree = _free.Count;
                    CountInUse = _inUse.Count;
                    _temp = obj.gameObject;
                    _temp.SetActive(true);
                    obj.SetFree(false);
                }

				if(NeedsCustomInit)
					_temp.GetComponent<IPoolInit>()?.InitFromPool();

                return _temp;
            }

            public void Release(ObjectPoolId obj)
            {
				if (NeedsCustomRelease)
					obj.GetComponent<IPoolRelease>()?.DeactivateBeforeRelease();

                _inUse.Remove(obj);
                CountInUse = _inUse.Count;
                _free.Add(obj);

                CountFree = _free.Count;

                obj.transform.SetParent(!InactiveParentGo ? Instance.transform : InactiveParentGo.transform);

                obj.SetFree(true);
                obj.gameObject.SetActive(false);
            }

            /// <summary>
            ///     Waits for 'cullDelay' in seconds and culls the 'despawned' list if
            ///     above 'cullingAbove' amount.
            ///     Triggered by DespawnInstance()
            /// </summary>
            public IEnumerator<float> CullDespawned()
            {
                // First time always pause, then check to see if the condition is
                //   still true before attempting to cull.
                yield return Timing.WaitForSeconds(CullWaitDelay);

                while (CountTotal > CullAboveCount)
                {
                    // Attempt to delete an amount == this.cullMaxPerPass
                    for (var i = 0; i < CullMaxPerPass; i++)
                    {
                        // Break if this.cullMaxPerPass would go past this.cullAbove
                        if (CountTotal <= CullAboveCount)
                            break; // The while loop will stop as well independently

                        // Destroy the last item in the list
                        if (CountFree <= 0) continue;

                        var inst = _free[0];
                        _free.RemoveAt(0);
                        Destroy(inst.gameObject);
                        CountFree = _free.Count;
                    }

                    // Check again later
                    yield return Timing.WaitForSeconds(CullWaitDelay);
                }

                // Reset the singleton so the feature can be used again if needed.
                CullingActive = false;
                yield return 0.0f;
            }

	        public void DestroyAll()
	        {
		        foreach (var e in _free)
		        {
					if (e.gameObject)
						Destroy(e.gameObject);
		        }
				_free.Clear();

		        foreach (var e in _inUse)
		        {
					if (e.gameObject)
						Destroy(e.gameObject);
		        }
				_inUse.Clear();

		        if (this._temp != null)
		        {
			        Destroy(_temp);
		        }
			}
        }
    }
}