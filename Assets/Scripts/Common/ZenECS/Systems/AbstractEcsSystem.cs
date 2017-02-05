// /** 
//  * AbstractEcsSystem.cs
//  * Will Hart
//  * 20161205
// */

namespace Zen.Systems
{
    #region Dependencies

    using System;
    using UnityEngine;
	using Zen.Common.ZenECS;

    #endregion

    public abstract class AbstractEcsSystem : IEcsSystem
    {
        [HideInInspector, NonSerialized]
	    private EcsEngine _engine;

        [HideInInspector]
	    protected EcsEngine engine
	    {
		    get
		    {
				if( _engine == null)
					_engine = EcsEngine.Instance;
			    Debug.Assert(_engine != null, "_engine != null");
				return _engine;
		    }
		    set { _engine = value; }
	    }

        public virtual bool Init()
        {
	        return true;
        }

        public virtual void Update()
        {
        }

        public virtual void FixedUpdate()
        {
        }

        public virtual void LateUpdate()
        {
        }

	    public override string ToString()
	    {
		    return this.GetType().Name;
	    }
    }
}