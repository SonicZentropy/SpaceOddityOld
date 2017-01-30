// /** 
// * TacticalAiMovementSystem.cs
// * Dylan Bailey
// * 20161211
// */


namespace Zen.Systems
{
	#region Dependencies

	using Common.Behaviours;
	using Common.ZenECS;
	using Components;
	using UnityEngine;

	#endregion

	public class TacticalAiMovementSystem : AbstractEcsSystem
	{
		/// <summary>
		///     Search for tactical AI who have a navigation target and start their pathfinding
		/// </summary>
		public override void Update()
		{
			var tacAis = engine.Get(ComponentTypes.TacticalAiStateComp);

			foreach (TacticalAiStateComp ai in tacAis)
			{
				// check if we are currently path finding or just navigating
				if (ai.IsFindingPath || !ai.NavigationTargetUpdated) continue;

				// find a new path if we need to
				ai.Owner.Wrapper.gameObject.GetComponent<AiPathFindingController>().GetPath(ai);
				ai.IsFindingPath = true;
				ai.NavigationTargetUpdated = false;

				if (!ai.Owner.GetComponent<MovementComp>().UseFixedUpdateMovement)
				{
					PerformMovement();
				}
			}
		}

		public override void FixedUpdate()
		{
			var tacAis = engine.Get(ComponentTypes.TacticalAiStateComp);

			foreach (TacticalAiStateComp ai in tacAis)
			{
				if (ai.Owner.GetComponent<MovementComp>().UseFixedUpdateMovement)
				{
					PerformMovement();
				}
			}
		}

		private void PerformMovement()
		{
			var tacAis = engine.Get(ComponentTypes.TacticalAiStateComp);

			foreach (TacticalAiStateComp ai in tacAis)
			{
				var move = ai.Owner.GetComponent<MovementComp>();
				var hasWaypoints = ai.Waypoints != null && ai.Waypoints.Count > 0;

				if (!hasWaypoints)
				{
					move.CurrentMoveSpeed = UpdateSpeed(false, move);
					if (ai.Action != null) ai.Action.IsComplete = true;
					return;
				}

				// check if we are at the end of the existing path
				if (ai.CurrentWaypoint >= ai.Waypoints.Count)
				{
					ai.CurrentWaypoint = 0;
					ai.Waypoints.Clear();
					return;
				}

				// navigate through waypoints
				var wp = ai.Waypoints[ai.CurrentWaypoint];
				var pos = ai.Owner.GetComponent<PositionComp>().Position;
				var dir = wp - pos;

				move.CurrentMoveSpeed = UpdateSpeed(true, move);

				var delta = dir.normalized * move.CurrentMoveSpeed * Time.deltaTime;
				delta.y = 0.01f; // make sure we don't bobble up and down or go through the floor

				var rb = ai.Owner.Wrapper.gameObject.GetComponent<Rigidbody>();
				rb.MovePosition(pos + delta);

				// _meshObject = context.State.Owner.Wrapper.gameObject.GetComponentInChildren<MeshRenderer>()?.gameObject;
				var _meshObject = ai.Owner.GetComponent<MeshComp>()?.MeshFilter?.gameObject;
				if (_meshObject != null && delta.sqrMagnitude > 0.1f)
				{
					var lookRot = 50 * delta;
					lookRot.y = 0;
					_meshObject.transform.rotation = Quaternion.Slerp(
					                                                  _meshObject.transform.rotation,
					                                                  Quaternion.LookRotation(lookRot, Vector3.up),
					                                                  Time.deltaTime * 5);
				}

				if (dir.x * dir.x + dir.z * dir.z < 2.5)
				{
					++ai.CurrentWaypoint;
				}
			}
		}

		protected virtual float UpdateSpeed(bool hasWaypoints, MovementComp move)
		{
			return hasWaypoints
				? move.MoveSpeed
				: 0;
		}
	}
}