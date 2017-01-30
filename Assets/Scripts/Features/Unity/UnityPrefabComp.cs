// /** 
//  * UnityPrefabComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zen.Components
{
    #region Dependencies

	using AdvancedInspector;
    using UnityEngine;
	using Zen.Common.Extensions;
	using Zen.Common.ZenECS;

    #endregion

    public class UnityPrefabComp : ComponentEcs
    {
        [Inspect, TextField(TextFieldType.Prefab)]
        public string PrefabLink = "Prefabs/None";

        [Inspect]
        public bool IsPooled { get; set; }

		//[Inspect]public UnityLayer layer { get; set; } = new UnityLayer(0);
	    public LayerMask layer;

	    [Enum(true)] public EntityTags entityTags;
	    [Enum(true)] public Tags tags;

	    public override string ToString()
	    {
		    return "UnityPrefabComp";
	    }

        public override ComponentTypes ComponentType => ComponentTypes.UnityPrefabComp;
    }
}