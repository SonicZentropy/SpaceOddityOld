﻿// /** 
//  * CreepNavigateToHero.cs
//  * Will Hart
//  * 20161103
// */

namespace Zenobit.AI.Actions
{
    #region Dependencies

    using System.Collections.Generic;

    using Axes;
    using Zenobit.AI.Core;
    using UnityEngine;
    using Components;
    using Common.ZenECS;
    using System.Linq;

    #endregion

    /// <summary>
    /// Sets navigation targets at set intervals
    /// </summary>
    public abstract class AbstractAttackingAction : AbstractNavigationAction
    {
        protected float PathfindingFrequency = 0.3f;
        protected float _nextPathfindingTime;

        protected AbstractAttackingAction(IEnumerable<IAxis> axes) : base(axes)
        {
        }

        /// <summary>
        /// Gets a navigation position within range of the assigned target
        /// </summary>
        /// <param name="context"></param>
        protected virtual Vector3 GetAttackingPosition(AiContext context)
        {
            var pos = context.GetComponent<PositionComp>();
            var combat = context.GetComponent<CombatComp>();

            var dirToTarget = (combat.TargetedEnemy.Value.Position - pos.Position).normalized;
            //var movementRange = combat.SelectedWeapon.Value.AttackRange;
	        var movementRange = 0f; //need to refactor above
            return movementRange * dirToTarget;
        }

        /// <summary>
        /// Invalidate expired or out of range targets
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual bool CheckTargetIsStillValid(AiContext context)
        {
            var pos = context.GetComponent<PositionComp>();
            var combat = context.GetComponent<CombatComp>();

            // check if our current enemy is valid
            if (combat.TargetedEnemy == null) return false;

            var dist = (pos.Position - combat.TargetedEnemy.Value.Position).sqrMagnitude;
            //if (!combat.TargetedEnemy.Value.Owner.GetComponent<HealthComp>().IsDead && 
            //    dist < combat.SelectedWeapon.Value.AttackRange) return true;
			//need to refactor above for combatcomp changes

            combat.TargetedEnemy = null;
            return false;
        }

        ///// <summary>
        ///// Finds a new target for the creep to fire at
        ///// </summary>
        ///// <param name="combat"></param>
        //protected virtual void UpdateTargetedComponent(CombatComp combat)
        //{
        //    var posComp = combat.Owner.GetComponent<PositionComp>();

        //    var nearest = EcsEngine.Instance.Get<PlayerMechComp>(ComponentTypes.PlayerMechComp)
        //        .Select(mech => mech.Owner.GetComponent<PositionComp>())
        //        .OrderBy(mechPos => (mechPos.Position - posComp.Position).sqrMagnitude)
        //        .FirstOrDefault();
        //    combat.TargetedEnemy = nearest;
        //}

        /// <summary>
        /// Run pathfinding perioidically to prevent thrashing the pathfinder
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override bool ShouldRunPathfinding(AiContext context)
        {
            if (Time.time < _nextPathfindingTime) return false;
            _nextPathfindingTime = Time.time + PathfindingFrequency;
            return true;
        }
    }
}