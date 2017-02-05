// /** 
//  * TargetComp.cs
//  * Dylan Bailey
//  * 20170205
// */

namespace Zen.Components
{
	using Common.ZenECS;
	using UnityEngine;

	public class TargetComp : ComponentEcs
	{
		public Transform target;

		public override ComponentTypes ComponentType => ComponentTypes.TargetComp;
		public override string Grouping => "Combat";
	}
}

