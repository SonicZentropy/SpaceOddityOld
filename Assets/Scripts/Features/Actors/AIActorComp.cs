// /**
//  * AIActorComp.cs
//  * Dylan Bailey
//  * 1/27/2017
// */

namespace Zen.Components
{
    #region Dependencies

    using UnityEngine;
    using Zen.Common.ZenECS;

    #endregion

    public class AIActorComp : AbstractActorComp
    {
	    [SerializeField]
	    private int _credits;

	    public int Credits
	    {
		    get { return _credits; }
		    set { _credits = value >= 0 ? value : 0; }
	    }

	    public const int playerID = 0;

	    public override ComponentTypes ComponentType => ComponentTypes.AIActorComp;
    }
}