/****************
* ShipContext.cs
* Dylan Bailey
* 2/1/2017
****************/

namespace Zen.AI.Apex.Contexts
{
	using global::Apex.AI;
	using UnityEngine;
	using Zen.Common.ZenECS;
	using Zen.Components;

	public class ShipContext  : ZenContext
	{

		public PositionComp positionComp { get; private set; }
		public Transform transform { get; private set; }
		public TargetComp targetComp { get; private set; }
		public ScannerComp scannerComp { get; private set; }
		public CommandComp commandComp { get; private set; }
		public Rigidbody rbComp { get; private set; }

		public AINavigationComp navComp { get; private set; }

		//public GameObject[] targets { get; private set; }

		public ShipContext(Entity inEntity)
			: base(inEntity)
		{
			positionComp = entity.GetComponent<PositionComp>();
			transform = positionComp.transform;
			targetComp = entity.GetComponent<TargetComp>();
			scannerComp = entity.GetComponent<ScannerComp>();
			commandComp = entity.GetComponent<CommandComp>();
			rbComp = entity.GetComponent<RigidbodyComp>()?.Rigidbody;
			navComp = entity.GetComponent<AINavigationComp>();
		}
	}
} 
