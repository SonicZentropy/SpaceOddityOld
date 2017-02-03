// /**
//  * ShipContextProviderComp.cs
//  * Dylan Bailey
//  * 2/1/2017
// */

namespace Zen.Components
{
    #region Dependencies

	using System;
	using Apex.AI;
	using Apex.AI.Components;
	using UnityEngine;
	using Zen.AI.Apex.Contexts;
	using Zen.Common.ZenECS;

	#endregion

    public class ShipContextProviderComp : ComponentEcs, IContextProvider
    {
	    public ShipContext context;

	    public IAIContext GetContext(Guid aiId)
	    {
		    return context;
	    }

	    public override ComponentTypes ComponentType => ComponentTypes.ShipContextProviderComp;
	    public override string Grouping => "AI";
    }
}