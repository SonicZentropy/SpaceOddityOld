namespace Zen.AI.Apex.Contexts
{
	using global::Apex.AI;
	using UnityEngine;
	using Zen.Components;

	public class TargetContext : IAIContext
	{
		public Transform transform { get; private set; }
		public TargetComp targetComp { get; private set; }
		public ScannerComp scannerComp { get; private set; }
		//public GameObject[] targets { get; private set; }

		public TargetContext(Transform inTransform, TargetComp tComp, ScannerComp sComp)
		{
			transform = inTransform;
			targetComp = tComp;
			scannerComp = sComp;
			//this.targets = targets;
		}
	}
}