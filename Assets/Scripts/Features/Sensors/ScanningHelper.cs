using System.Collections.Generic;
using UnityEngine;
using Zenobit.Common.Extensions;
using Zenobit.Common.ZenECS;

public static class ScanningHelper 
{
	public static List<EntityWrapper> FindEntitiesInRange(Vector3 centerPosition, float radiusOfScan, int desiredMask = -1)
	{
		Collider[] hits;

		if (desiredMask == -1)
			hits = Physics.OverlapSphere(centerPosition, radiusOfScan);
		else
		{
			hits = Physics.OverlapSphere(centerPosition, radiusOfScan, desiredMask);
		}
		
		List<EntityWrapper> EntitiesInRange = new List<EntityWrapper>(10);

		for (int i = 0; i < hits.Length; i++)
		{
			if (hits[i].gameObject.HasEntityTag(EntityTags.IsEntity))
				EntitiesInRange.Add(hits[i].gameObject.GetEntityWrapper());
		}
		return EntitiesInRange;
	}
}
