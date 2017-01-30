// /** 
//  * TacticalAiStateComp.cs
//  * Will Hart
//  * 20161110
// */

namespace Zen.Components
{
    #region Dependencies

    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using Zen.AI.Actions;
    using Zen.AI.Core;
    using Zen.Common.ZenECS;

    #endregion

    public class TacticalAiStateComp : ComponentEcs
    { 
        public float NextAiPlanTime { get; set; }

        public Vector3 NavigationTarget {get; set;}
        [NonSerialized] public bool NavigationTargetUpdated;
    
        [NonSerialized] public Guid AttackTargetHealth;
	    [NonSerialized] public bool AttackTargetUpdated;

        [NonSerialized] public bool IsFindingPath;
        [NonSerialized] public List<Vector3> Waypoints = new List<Vector3>();
        [NonSerialized] public int CurrentWaypoint;
    
        [NonSerialized] public AbstractAiAction Action;
        [NonSerialized] public AiActionContainer ActionContainer;
        
        public override ComponentTypes ComponentType => ComponentTypes.TacticalAiStateComp;
    }
}