// /** 
//  * AiActionBundle.cs
//  * Will Hart
//  * 20161103
// */

namespace Zenobit.AI.Bundles
{
    #region Dependencies

    using System.Collections.Generic;
    using Actions;

    #endregion

    public abstract class AiActionBundle
    {
        private readonly List<AbstractAiAction> _actions = new List<AbstractAiAction>();

        protected AiActionBundle(IEnumerable<AbstractAiAction> actions)
        {
            _actions.AddRange(actions);
            _actions.Sort((a, b) => b.Priority.CompareTo(a.Priority));
        }

        public IEnumerable<AbstractAiAction> Actions => _actions;
    }
}