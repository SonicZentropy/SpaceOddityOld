// /** 
//  * CreepNavigateToHero.cs
//  * Will Hart
//  * 20161103
// */

namespace Zenobit.AI.Actions
{
    #region Dependencies

    using System.Collections.Generic;

    using Axes;
    using Core;
    using UnityEngine;

    using Zenobit.Components;
    using Common.ZenECS;

    #endregion

    /// <summary>
    /// Sets navigation targets at set intervals
    /// </summary>
    public abstract class AbstractNavigationAction : AbstractAiAction
    {
        //protected GameObject _meshObject;

        protected AbstractNavigationAction(IEnumerable<IAxis> axes) : base(axes)
        {
        }

		// Removed b/c no need for meshObject now that movement is relocated to system
        //public override void OnEnter(AiContext context)
        //{
        //    base.OnEnter(context);
        //    _meshObject = context.State.Owner.Wrapper.gameObject.GetComponentInChildren<MeshRenderer>()?.gameObject;
        //}

        public override void Update(AiContext context)
        {
            base.Update(context);
			if (ShouldRunPathfinding(context))
			{
				RunPathFinding(context);
			}
		}

        protected abstract bool ShouldRunPathfinding(AiContext context);

		//Made abstract because the previous implementation only suitable for the RTS game
	    protected abstract Vector3 GetPathFindingTarget(AiContext context);
        //protected virtual Vector3 GetPathFindingTarget(AiContext context)
        //{
	    //    var cc = context.GetComponent<CreepComp>();
	    //    PositionComp ah = cc.AssignedHero;
	    //    return ah.Position;
        //}

        protected virtual void RunPathFinding(AiContext context)
        {
            // the path is handled by the TacticalAiMovementSystem, this just sets the destination
            var previous = context.State.NavigationTarget;
            context.State.NavigationTarget = GetPathFindingTarget(context);

            // don't pathfind for very small differences - an arbitrary number which prevents jittering and spurious pathing
            context.State.NavigationTargetUpdated = (previous - context.State.NavigationTarget).sqrMagnitude > 0.1f;
        }

        
    }
}