﻿// /**
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
	    public bool ShouldMove = true;
	    public bool HasReachedTarget = true;
	    public Vector3 TargetPositionOffset;

        public override ComponentTypes ComponentType => ComponentTypes.AINavigationComp;
	    public override string Grouping => "AI";
    }
}