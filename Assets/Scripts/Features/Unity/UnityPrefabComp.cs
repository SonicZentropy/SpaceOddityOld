// /** 
//  * UnityPrefabComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zenobit.Components
{
    #region Dependencies

    using AdvancedInspector;
    using Pathfinding.Serialization;
    using UnityEngine;
    using Zenobit.Common.ZenECS;

    #endregion

    public class UnityPrefabComp : ComponentEcs
    {
        [Inspect] [TextField(TextFieldType.Prefab)] public string PrefabLink = "Prefabs/None";

        [Inspect]
        public bool IsPooled { get; set; }

		//[Inspect]public UnityLayer layer { get; set; } = new UnityLayer(0);
	    public LayerMask layer;


        public override ComponentTypes ComponentType => ComponentTypes.UnityPrefabComp;
    }
}