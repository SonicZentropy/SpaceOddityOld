/****************
* RandomlyOrbitTargetAction.cs
* Dylan Bailey
* 2/3/2017
****************/

namespace Zen.AI.Apex.Actions
{
	#region Dependencies

	using global::Apex.AI;
	using global::Apex.Serialization;
	using global::Apex.Serialization.Converters;
	using UnityEngine;
	using Zen.AI.Apex.Contexts;
	using Zen.Common.Extensions;
	using Zen.Components;
	using ZR = Common.Extensions.ZenUtils.RandUtil;

	#endregion

	public class RandomlyOrbitTargetAction : ActionBase<ShipContext>
	{
	    [ApexSerialization(defaultValue = 6f), FriendlyName("MinOrbitRange", "Minimum range of orbit positions")]
	    public float MinOrbitRange = 6f;

	    [ApexSerialization(defaultValue = 20f), FriendlyName("MaxOrbitRange", "Maximum range of possible orbit positions")]
	    public float MaxOrbitRange = 20f;


		public override void Execute(ShipContext context)
		{
			//Debug.Log($"In randomly orbit action");
			if (context.targetComp.target != null)
			{
				var navComp = context.navComp;
				navComp.AINavState = EAINavState.ORBIT;
				navComp.HasReachedTarget = false;
                //navComp.TargetPositionOffset = ZenUtils.Vec3Util.GetRandomVector(3, 3, 3);
                //navComp.TargetPositionOffset = GetRandomOrbitPosition(15, 6);

                //Find new orbit point with no immediate collisions
			    bool FoundFreePoint = false;
			    while (!FoundFreePoint)
			    {
			        //Try getting vector on the other side of the target diff
			        navComp.TargetPositionOffset = (context.targetComp.target.position - context.transform.position)
			                                       + GetRandomOrbitPosition(MinOrbitRange, MaxOrbitRange);
			        if (!Physics.Linecast(context.transform.position,
			                              context.targetComp.target.position + navComp.TargetPositionOffset,
			                              ZenUtils.LayerMaskFromIDs(SRLayers.foreground, SRLayers.npc, SRLayers.player)))
			        {
			            FoundFreePoint = true;
			        }
			        else
			        {
			            Debug.Log("Point is collision course");
			        }
			    }
            }
		}

		private Vector3 GetRandomOrbitPosition(float minOrbitRange, float maxOrbitRange)
		{
			return new Vector3(Random.Range(minOrbitRange, maxOrbitRange) * ZR.GetRandomSign(),
			                    Random.Range(minOrbitRange, maxOrbitRange) * ZR.GetRandomSign(),
			                    Random.Range(minOrbitRange, maxOrbitRange) * ZR.GetRandomSign());
		}
	}
}