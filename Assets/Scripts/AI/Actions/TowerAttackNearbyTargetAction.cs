// /** 
//  * TowerAttackNearbyTargetAction.cs
//  * Will Hart
//  * 20161116
// */

namespace Zen.AI.Actions
{
    #region Dependencies

    using Axes;
    using Components;
    using Core;
    using System.Collections.Generic;
    using UnityEngine;

	#endregion

    public class TowerAttackNearbyTargetAction : AbstractAttackingAction
    {
        public TowerAttackNearbyTargetAction() : base(new List<IAxis>
        {
            new HasNearbyTargetsAxis()
        })
        {
        }

        public override int Priority => 1;

        protected override bool ShouldRunPathfinding(AiContext context) => false;
	    
		// NONWORKING now with NavigationAction refactor which relocated movement to systems
	    protected override Vector3 GetPathFindingTarget(AiContext context)
	    {
		    throw new System.NotImplementedException();
	    }

        protected float UpdateSpeed(bool hw, MovementComp m, AiContext c) => 0;

        public override void Update(AiContext context)
        {
            if (!CheckTargetIsStillValid(context))
            {
                IsComplete = true;
            }
        }
    }
}