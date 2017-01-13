// /** 
//  * CreepAiBundle.cs
//  * Will Hart
//  * 20161103
// */

namespace Zenobit.AI.Bundles
{
    #region Dependencies

    using System.Collections.Generic;
    using Zenobit.AI.Actions;

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