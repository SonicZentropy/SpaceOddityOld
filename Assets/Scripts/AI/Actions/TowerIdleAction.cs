// /** 
//  * TowerIdleAction.cs
//  * Will Hart
//  * 20161116
// */

namespace Zen.AI.Actions
{
    #region Dependencies

    using System.Collections.Generic;
    using Zen.AI.Axes;

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