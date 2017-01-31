// /** 
// * TacticalAiPlanningSystem.cs
// * Dylan Bailey
// * 20161211
// */


//#define AI_DEBUG

namespace Zen.Systems
{
	#region Dependencies

	using AI.Bundles;
	using AI.Core;
	using Common.ZenECS;
	using Components;
	using UnityEngine;
#if AI_DEBUG
	using System.Linq;
#endif

	#endregion

	public class TacticalAiPlanningSystem : AbstractEcsSystem
	{
		/// <summary>
		///     Build context and periodically plan current action
		/// </summary>
		public override void Update()
		{
#if AI_DEBUG
			var planners = engine.Get<TacticalAiStateComp>(ComponentTypes.TacticalAiStateComp).ToList();
			Debug.Log($"Planning actions for {planners.Count} entities");
#else
	// leave result as IEnumerable if we aren't debugging      
            var planners = engine.Get(ComponentTypes.TacticalAiStateComp);
#endif

			foreach (TacticalAiStateComp planner in planners)
			{
				if (ErrorInCreatingActionContainer(planner)) continue;

				var context = new AiContext(planner.Owner);

				// run the action
				if (planner.Action != null)
				{
					planner.Action.Update(context);

					// check if the action is complete
					if (planner.Action.IsComplete)
					{
						planner.Action.OnExit(context);
						planner.Action = null;
					}
				}

				// only replan the ai if required
				if ((planner.Action != null) && (Time.time <= planner.NextAiPlanTime)) continue;

				// do not interrupt timed actions, let them run to completion
				if ((planner.Action != null) && planner.Action.IsTimed) continue;

				// if the action has changed, exit the previous action
				var newAction = planner.ActionContainer.GetActiveAction(context);
				if (newAction == planner.Action) continue;

				planner.Action?.OnExit(context);
				newAction.OnEnter(context);
				planner.Action = newAction;
			}
		}

		
		private static bool ErrorInCreatingActionContainer(TacticalAiStateComp planner)
		{
			if (planner.ActionContainer != null) return false;

			planner.ActionContainer = new AiActionContainer();

			if (planner.Owner.HasComponent(ComponentTypes.CreepComp))
			{
				planner.ActionContainer.AddBundle(new CreepAiBundle());
			}
			else
			{
				ZenLogger.LogWarning($"Unable to select an appropriate AI bundle for entity {planner.Owner.EntityName}, ignoring");
				return true;
			}

			return false;
		}

	}
}