namespace Zenobit.Components
{
	using System.Collections.Generic;
	using Systems;
	using AdvancedInspector;
	using Common.ZenECS;
	using UnityEngine;

	public class SectorGenerationComp : ComponentEcs
	{
		[Inspect]
		[TextField(TextFieldType.Prefab)]
		public string StarPrefab = "Prefabs/None";
		public bool renderCentralStar;
		public Vector3 centralStarLocation;

		public int sectorSize = 300;
		public int sectorHeight = 100;

		public float shipTriggerDistance = 96f;
		public float objectTriggerDistance = 64f;

		[ReadOnly]
		public float shipTriggerDistanceSquared;
		[ReadOnly]
		public float objectTriggerDistanceSquared;

		public bool DisableOnCreate = false;

		[SerializeField][Inspect]
		public List<ObjectGenerationContainer> spawnObjList = new List<ObjectGenerationContainer>();

		public override ComponentTypes ComponentType => ComponentTypes.SectorGenerationComp;
	}
}