// /** 
//  * TowerIdleAction.cs
//  * Will Hart
//  * 20161116
// */

namespace Zenobit.AI.Actions
{
    #region Dependencies

    using System.Collections.Generic;
    using Zenobit.AI.Axes;

    #endregion

    public class TowerIdleAction : AbstractAiAction
    {
        public TowerIdleAction() : base(new List<IAxis>
        {
            new InvertAxis(new HasNearbyTargetsAxis())
        })
        {
        }

        public override int Priority => 1;
    }
}