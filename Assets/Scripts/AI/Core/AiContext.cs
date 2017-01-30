// /** 
//  * AiContext.cs
//  * Will Hart
//  * 20161103
// */

namespace Zen.AI.Core
{
    using Zen.Common.ZenECS;
    using Zen.Components;

    /// <summary>
    ///     A class that contains information suitable for the AI context
    /// </summary>
    public class AiContext
    {
        private readonly Entity _entity;

        public AiContext(Entity entity)
        {
            _entity = entity;
            State = _entity.GetComponent<TacticalAiStateComp>();
        }

        /// <summary>
        ///     Gets a reference to the world state
        /// </summary>
        public TacticalAiStateComp State { get; }

        /// <summary>
        ///     Gets a component from the attached entity
        ///     A convenience method to avoid calling .Owner repeatedly
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetComponent<T>() where T : ComponentEcs
        {
            return _entity.GetComponent<T>();
        }
    }
}