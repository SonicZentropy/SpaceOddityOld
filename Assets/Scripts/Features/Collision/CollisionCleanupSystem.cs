// /** 
//  * CollisionCleanupSystem.cs
//  * Dylan Bailey
//  * 20161210
// */

namespace Zen.Systems
{
    #region Dependencies

    using Zen.Common.ZenECS;
    using Zen.Components;

    #endregion

    public class CollisionCleanupSystem : AbstractEcsSystem
    {
        public override bool Init()
        {
            return true;
        }

        public override void Update()
        {
            var collenter = engine.Get(ComponentTypes.CollisionEnterComp);
            foreach (CollisionEnterComp col in collenter)
            {
                col.Other.Clear();
            }

            var collexit = engine.Get(ComponentTypes.CollisionExitComp);

			foreach (CollisionExitComp col in collexit)
            {
                col.Other.Clear();
            }
        }
    }
}