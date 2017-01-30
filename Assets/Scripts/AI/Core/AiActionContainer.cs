// /** 
//  * AiActionContainer.cs
//  * Will Hart
//  * 20161103
// */

//#define AI_DEBUG

namespace Zen.AI.Core
{
    #region Dependencies

	using System.Collections.Generic;
	using UnityEngine;

//#if UNITY_EDITOR
    using System.Collections.ObjectModel;
//#endif

    using Zen.AI.Actions;
    using Zen.AI.Bundles;
	using Random = UnityEngine.Random;

    #endregion

    /// <summary>
    /// This class represents the AI for a single entity. It is reponsible for holding 
    /// current actions and managing bundles of actions that can be added and 
    /// removed from the entity's AI planning system
    /// </summary>
    public class AiActionContainer
    {
        private const float AiPlanPeriod = 1f;

        private readonly List<AbstractAiAction> _actions = new List<AbstractAiAction>();

        /// <summary>
        /// Constructs an AI without any actions
        /// </summary>
        public AiActionContainer()
        {
        }

        /// <summary>
        /// Default constructor, creates an AI with the given list of actions
        /// </summary>
        /// <param name="actions"></param>
        public AiActionContainer(IEnumerable<AbstractAiAction> actions)
        {
            _actions.AddRange(actions);
            _actions.Sort((a, b) => b.Priority.CompareTo(a.Priority));
        }

//#if UNITY_EDITOR
        public ReadOnlyCollection<AbstractAiAction> ActionBundle => _actions.AsReadOnly();
//#endif

        /// <summary>
        /// Gets a single active action with the highest priority, typically used for tactical AI
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public AbstractAiAction GetActiveAction(AiContext context)
        {
#if AI_DEBUG
            Debug.Log("Running AI planning");
#endif
            var best = 0f;
            AbstractAiAction bestAction = null;

            foreach (var action in _actions)
            {
                if (best > action.Priority)
                {
#if AI_DEBUG
                    Debug.Log("Breaking out of planning action - priority too low");
#endif
                    break; // cannot possibly beat the score
                }

                var newScore = action.GetScore(context);
#if AI_DEBUG
                Debug.Log($"Calculated score {newScore} for {action.GetType()}");
#endif

                if (newScore < best) continue;

                best = newScore;
                bestAction = action;
            }

            context.State.NextAiPlanTime = Time.time
                                           + AiPlanPeriod - 0.5f + Random.value;

#if AI_DEBUG
            Debug.Log(
                bestAction == null
                    ? "No action selected - bestAction is null"
                    : $"New action selected by AI Planner {bestAction.GetType()} with score {best}");
#endif

            return bestAction;
        }

        /// <summary>
        /// Adds a bundle of AI actions to the ActionContainer
        /// </summary>
        /// <param name="bundle"></param>
        public void AddBundle(AiActionBundle bundle)
        {
            _actions.AddRange(bundle.Actions);
            _actions.Sort((a, b) => b.Priority.CompareTo(a.Priority));
        }

        /// <summary>
        /// Clears all actions from the ActionContainer
        /// </summary>
        private void ClearAllActions()
        {
            _actions.Clear();
        }

        /// <summary>
        /// Replaces all the existing actions with the given action bundle
        /// </summary>
        /// <param name="bundle"></param>
        public void ReplaceActions(AiActionBundle bundle)
        {
            ClearAllActions();
            AddBundle(bundle);
        }
    }
}