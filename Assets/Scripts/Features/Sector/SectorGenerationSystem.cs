// /** 
// * SectorGenerationSystem.cs
// * Dylan Bailey
// * 20161213
// */
#pragma warning disable 0618, 0649, 0414

namespace Zen.Systems
{
	#region Dependencies

	using System;
	using System.Linq;
	using AdvancedInspector;
	using Common.ObjectPool;
	using Common.ZenECS;
	using Components;
	using UnityEngine;

	#endregion

	public class SectorGenerationSystem : AbstractEcsSystem
	{
		private GameObject current;

		private System.Random srng;
		private GameObject playerShip;


		private int numObjects;
		private int sectorMin;
		private int sectorMax;

//		private Vector3 pShipPos = new Vector3();
		private Vector3 objPos = new Vector3();
		
		Vector3 pos = new Vector3();
		private int mask;
		private int[,] arrayObjs;
//		private bool bPanic = false;
		private static WaitForSeconds wait = new WaitForSeconds(1.5f);

		private SectorGenerationComp sg;
		
		public override bool Init()
		{
			sg = engine.Get<SectorGenerationComp>(ComponentTypes.SectorGenerationComp).First();
			
			//numObjects = numRounds * (numBrownAst + numGreyAst + numNPCShip + numStars);
			numObjects = 0;

			if (sg.spawnObjList == null) throw new InitializeFailedException();

			foreach (var spawnObj in sg.spawnObjList)
			{
				numObjects += spawnObj.numberToSpawn;
			}

			sectorMin = -sg.sectorSize;
			sectorMax = sg.sectorSize;

			arrayObjs = new int[sectorMax * 2, sectorMax * 2];
			playerShip = GameObject.Find("PlayerShip");

			sg.shipTriggerDistanceSquared = sg.shipTriggerDistance * sg.shipTriggerDistance;
			sg.objectTriggerDistanceSquared = sg.objectTriggerDistance * sg.objectTriggerDistance;

			//From Start

			mask = LayerMask.GetMask("foreground", "npc");
			srng = new System.Random(1);
			pos.Set(5, 5, 5);
			if (sg.renderCentralStar)
			{
				//current = starPrefab.InstantiateFromPool(pos, Quaternion.identity);
				//var enab = current.GetComponentDownward<DisableByDistanceComp>();
				//if (enab)
				//{
				//	if (DisableOnCreate) enab.DisableThisObject();
				//	else enab.EnableThisObject();
				//}
				current = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(sg.StarPrefab));
				current.transform.position = sg.centralStarLocation;
			}

			foreach (var obj in sg.spawnObjList)
			{
				if (obj.spawnEnabled)
					CreateNewObject(obj);
			}

			return false;
		}
		
		void CreateNewObject(ObjectGenerationContainer ogc, Transform parentObj = null)
		{
			if (ogc.numberToSpawn <= 0) return;

			for (int i = 0; i < ogc.numberToSpawn; i++)
			{
				do
				{
					pos.Set(srng.Next(sectorMin, sectorMax), srng.Next(-sg.sectorHeight/2, sg.sectorHeight/2), srng.Next(sectorMin, sectorMax));
				} while (arrayObjs[(int)pos.x + sectorMax, (int)pos.y + sectorMax] == 1);
				arrayObjs[(int)pos.x + sectorMax, (int)pos.y + sectorMax] = 1;

				current = CreateGameObjectFromContainer(ogc);
				current.transform.position = pos;

				if (parentObj != null) current.transform.parent = parentObj.transform;
			}

		}

		public GameObject CreateGameObjectFromContainer(ObjectGenerationContainer ogc)
		{
			if (ogc.IsEntity)
			{
				var ent = engine.CreateEntity(ogc.EntityToGenerate);
				return ent.Wrapper.gameObject;
			}

			GameObject go = Resources.Load<GameObject>(ogc.PrefabToGenerate);
			if (ogc.ShouldPool)
			{
				return go.InstantiateFromPool();
			}
			return UnityEngine.Object.Instantiate(go);
		}
	}

	[Serializable]
	public class ObjectGenerationContainer
	{
		[Inspect("IsNotEntity")]
		[TextField(TextFieldType.Prefab)]
		public string PrefabToGenerate = "Prefabs/None";

		[Inspect("IsEntity")] [TextField(TextFieldType.Entity)] public string EntityToGenerate = "Entities/None";

		public int numberToSpawn;
		public bool spawnEnabled;
		
		[Inspect]public bool IsEntity { get; set; }
		[Inspect]public bool ShouldPool { get; set; }

		private bool IsNotEntity()
		{
			return !IsEntity;
		}

		public ObjectGenerationContainer()
		{
			numberToSpawn = 0;
			spawnEnabled = true;
		}

		public ObjectGenerationContainer(string inObject, int inSpawnNum, bool inEnabled)
		{
			PrefabToGenerate = inObject;
			numberToSpawn = inSpawnNum;
			spawnEnabled = inEnabled;
		}

		public override string ToString()
		{
			if (PrefabToGenerate == null)
				return "Empty Spawn Object";
			return PrefabToGenerate;
		}
	}
}