// /** 
//  * CreepComp.cs
//  * Will Hart
//  * 20161104
// */

namespace Zenobit.Components
{

    #region Dependencies

    using System;
    using Zenobit.Common.ZenECS;
    using UnityEngine;

    #endregion

    public class CreepComp : ComponentEcs
    {
        public int CreepId { get; set; }
        public GuidLink<PositionComp> AssignedHero { get; set; } = new GuidLink<PositionComp>();
        public float ConstructionCost { get; set; }

        [NonSerialized]
        public GameObject CreepObject;
        
        public override ComponentTypes ComponentType => ComponentTypes.CreepComp;
    }
}