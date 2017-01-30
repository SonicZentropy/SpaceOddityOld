/*
// / ** 
// * PlayerMechNavigateToPositionAction.cs
// * Dylan Bailey
// * 20161215
// * /

namespace Zen.AI.Actions
{
	#region Dependencies

	using System.Collections.Generic;
	using Axes;
	using Components;
	using Core;
	using UnityEngine;

	#endregion

	public class PlayerMechNavigateToPositionAction : AbstractNavigationAction
	{
		private const float PathfindingFrequency = 1.5f;
		private float _nextPathfindingTime;

		public PlayerMechNavigateToPositionAction()
			: base(new List<IAxis>
			       {
				       new InvertAxis(new IsNearFocusPositionAxis())
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

		protected override Vector3 GetPathFindingTarget(AiContext context)
		{
			var pm = context.GetComponent<PlayerMechComp>();
			var offset = context.GetComponent<FormationComp>();
			PositionComp ah = pm.FocusObject;
			// Add formation offset for mighty ducks functionality
			return ah.Position + offset.OffsetPos;
		}

		protected override bool ShouldRunPathfinding(AiContext context)
		{
			if (Time.time < _nextPathfindingTime) return false;
			_nextPathfindingTime = Time.time + PathfindingFrequency;
			return true;
		}
	}
}*/