// /** 
//  * CreepAiBundle.cs
//  * Will Hart
//  * 20161103
// */

namespace Zen.AI.Bundles
{
    #region Dependencies

    using System.Collections.Generic;
    using Zen.AI.Actions;

    #endregion

    public class CreepAiBundle : AiActionBundle
    {
        public CreepAiBundle() : base(new List<AbstractAiAction>
        {
            new CreepNavigateToHeroAction()
        })
        {
        }
    }
}