// /** 
//  * PositionComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zenobit.Components
{
    #region Dependencies

    using System;
    using AdvancedInspector;
    using UnityEngine;
    using Zenobit.Common.ZenECS;

    #endregion

    public class PositionComp : ComponentEcs
    {
        //PH, we'll want to inject this from the Entity
        [NonSerialized] public Transform transform;
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
		[Inspect]public bool UseLateUpdate { get; set; }

        public override ComponentTypes ComponentType => ComponentTypes.PositionComp;
    }
}