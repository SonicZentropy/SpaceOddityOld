/****************
* TargetContextProvider.cs
* Dylan Bailey
* 1/31/2017
****************/

namespace Zen.AI.Apex.Contexts
{
	using System;
	using System.Collections.Generic;
	using global::Apex.AI;
	using global::Apex.AI.Components;
	using MEC;
	using UnityEngine;
	using Zen.Common.Extensions;
	using Zen.Common.ZenECS;
	using Zen.Components;

	public class TargetContextProvider : MonoBehaviour, IContextProvider
	{
		private TargetContext _context;

		//[SerializeField] private GameObject[] _targets = new GameObject[0];

		public IAIContext GetContext(Guid aiId)
		{
			return _context;
		}

		public void Start()
		{
			//var targs = Physics.OverlapSphere(transform.position, 200f);
			//List<GameObject> targGOs = new List<GameObject>();
			//foreach (var tg in targs)
			//{
			//	if (tg.gameObject != this.gameObject)
			//	{
			//		targGOs.Add(tg.gameObject);
			//	}
			//}
			ZenLogger.Log($"enabling target context provider");
			//ZenUtils.ExecuteAtEndOfFrame(() =>
			//                             {
			//	                             ZenLogger.Log($"In end of frame");
			//	                             var ent = gameObject.GetEntity();
			//	                             TargetComp tComp = ent.GetComponent<TargetComp>();
			//	                             ScannerComp sComp = ent.GetComponent<ScannerComp>();
			//	                             _context = new TargetContext(transform, tComp, sComp);
			//                             });
			Timing.RunCoroutine(_ExecuteAtEndOfFrame(), Segment.LateUpdate);
		}

		private IEnumerator<float> _ExecuteAtEndOfFrame()
		{
			ZenLogger.Log($"In execute at end of frame");
			EntityWrapper ew = null;

			while (ew == null)
			{
				ew = gameObject.GetComponentInChildren<EntityWrapper>();

				var ent = ew?.Entity;
				//Debug.Break();
				//var ent = gameObject.GetEntity();
				if (ent == null)
				{
					ZenLogger.Log($"no entity");
				}
				TargetComp tComp = ent?.GetComponentOrNull<TargetComp>();
				ScannerComp sComp = ent?.GetComponentOrNull<ScannerComp>();
				ZenLogger.Log($"Creating new context");
				_context = new TargetContext(transform, tComp, sComp);
				//Debug.Break();
				yield return 0f;
			}
			yield return 0f;
		}

		//_context = new TargetContext(transform, targGOs.ToArray());
	}
}