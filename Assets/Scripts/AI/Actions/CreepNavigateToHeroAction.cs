// /** 
//  * CreepNavigateToHero.cs
//  * Will Hart
//  * 20161103
// */

namespace Zen.AI.Actions
{
    #region Dependencies

    using System.Collections.Generic;

    using Axes;
    using Components;
    using Zen.AI.Core;
    using UnityEngine;

    #endregion

    public class CreepNavigateToHeroAction : AbstractNavigationAction
    {
        private const float PathfindingFrequency = 1.5f;
        private float _nextPathfindingTime;

        public CreepNavigateToHeroAction() : base(new List<IAxis>
        {
            new InvertAxis(new IsNearHeroAxis())
        })
        {
        }

        public override int Priority => 1;

        public override void OnEnter(AiContext context)
        {
            base.OnEnter(context);
            RunPathFinding(context);
            _nextPathfindingTime = Time.time + PathfindingFrequency;
        }

        protected override bool ShouldRunPathfinding(AiContext context)
        {
            if (Time.time < _nextPathfindingTime) return false;
            _nextPathfindingTime = Time.time + PathfindingFrequency;
            return true;
        }

	    protected override Vector3 GetPathFindingTarget(AiContext context)
	    {
			var cc = context.GetComponent<CreepComp>();
			PositionComp ah = cc.AssignedHero;
			return ah.Position;
		}
	}
}