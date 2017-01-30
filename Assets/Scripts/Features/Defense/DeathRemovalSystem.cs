// /** 
//  * DeathRemovalSystem.cs
//  * Dylan Bailey
//  * 20161109
// */

namespace Zen.Systems
{
    #region Dependencies

	using Features.Explosions;
	using Zen.Common;
    using Zen.Common.ZenECS;
    using Zen.Components;

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