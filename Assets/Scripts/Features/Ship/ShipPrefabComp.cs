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
	using Zen.Common.ZenECS;

	#endregion

	public class ShipPrefabComp : ComponentEcs
	{
		[Inspect]
		[TextField(TextFieldType.Prefab, "Ships")]
		public string ShipPrefab = "Prefabs/None";



		[Inspect]
		public bool IsPooled { get; set; } = true;

		[Inspect]
		public Vector3 FirstPersonCameraOffset { get; set; }

		public override ComponentTypes ComponentType => ComponentTypes.ShipPrefabComp;
		public override string Grouping => "Ship";
	}
}