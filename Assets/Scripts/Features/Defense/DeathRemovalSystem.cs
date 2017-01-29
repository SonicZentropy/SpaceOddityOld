// /** 
//  * DeathRemovalSystem.cs
//  * Will Hart and Dylan Bailey
//  * 20161109
// */

namespace Zenobit.Systems
{
    #region Dependencies

	using Features.Explosions;
	using Zenobit.Common;
    using Zenobit.Common.ZenECS;
    using Zenobit.Components;

    #endregion

    /// <summary>
    ///     Removes dead units
    /// </summary>
    public class DeathRemovalSystem : AbstractEcsSystem
    {
	    public override bool Init()
	    {
		    return true;
	    }

	    public override void Update()
	    {
		    var hcs = engine.Get(ComponentTypes.HullComp);
		    for (int i = hcs.Count -1; i >= 0 ; i--)
		    {
			    if (((HullComp) hcs[i]).CurrentHull <= 0)
			    {
				    Explosions.Create(Res.Prefabs.Explosion_01, hcs[i].GetComponent<PositionComp>().Position);
				    engine.DestroyEntity(hcs[i].Owner);
			    }
		    }
	    }
    }
}