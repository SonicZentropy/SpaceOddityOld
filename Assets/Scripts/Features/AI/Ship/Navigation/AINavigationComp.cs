// /**
//  * AINavigationComp.cs
//  * Dylan Bailey
//  * 2/2/2017
// */

namespace Zen.Components
{
    #region Dependencies

    using UnityEngine;
    using Zen.Common.ZenECS;

	#endregion

    public class AINavigationComp : ComponentEcs
    {
	    public bool ShouldMove { get; set; }

	    private bool hasReachedTarg = true;

	    public bool HasReachedTarget
	    {
		    get
		    {
			    return hasReachedTarg;
		    }
		    set
		    {
			    hasReachedTarg = value;
		    }
	    }

	    public Vector3 TargetPositionOffset;

        public override ComponentTypes ComponentType => ComponentTypes.AINavigationComp;
	    public override string Grouping => "AI";
    }
}