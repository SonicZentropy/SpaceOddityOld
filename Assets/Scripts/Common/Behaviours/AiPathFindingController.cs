// /** 
//  * AiPathFindingController.cs
//  * Will Hart
//  * 20161103
// */

namespace Zenobit.Common.Behaviours
{
    #region Dependencies

	using System;
	using Pathfinding;
    using UnityEngine;
    using ZenECS;
    using Zenobit.Components;

    #endregion

    public class AiPathFindingController : ZenBehaviour, IOnAwake
    {
        private Seeker _seeker;
        private TacticalAiStateComp _ai;

        public void GetPath(TacticalAiStateComp ai)
        {
            _ai = ai;

			_seeker.StartPath(ai.Owner.GetComponent<PositionComp>().Position, ai.NavigationTarget, ReceivePath);
        }

        private void ReceivePath(Path p)
        {
            _ai.IsFindingPath = false;
            _ai.CurrentWaypoint = 0;
            _ai.Waypoints = p.vectorPath;
        }

        public void OnAwake()
        {
            _seeker = GetComponent<Seeker>();
        }

	    public override int ExecutionPriority { get; } = -10;
	    public override Type ObjectType { get; } = typeof(AiPathFindingController);
    }
}
