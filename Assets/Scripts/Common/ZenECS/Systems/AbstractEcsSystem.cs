// /** 
//  * AbstractEcsSystem.cs
//  * Will Hart
//  * 20161205
// */

namespace Zenobit.Systems
{
    #region Dependencies

	using UnityEngine;
	using Zenobit.Common.ZenECS;

    #endregion

    public abstract class AbstractEcsSystem : IEcsSystem
    {
	    private EcsEngine _engine;

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
    }
}