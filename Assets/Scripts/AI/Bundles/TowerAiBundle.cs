// /** 
//  * TowerAiBundle.cs
//  * Will Hart
//  * 20161103
// */

namespace Zenobit.AI.Bundles
{
    #region Dependencies

    using System.Collections.Generic;

    using Actions;

    #endregion

    public class TowerAiBundle : AiActionBundle
    {
        public TowerAiBundle() : base(new List<AbstractAiAction>
        {
            new TowerIdleAction(),
            new TowerAttackNearbyTargetAction()
        })
        {
        }
    }
}