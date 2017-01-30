// /** 
//  * AlwaysOneAxis.cs
//  * Will Hart
//  * 20161103
// */

namespace Zen.AI.Axes
{
    #region Dependencies

    using Zen.AI.Core;

    #endregion

    public class AlwaysOneAxis : IAxis
    {
        public float Score(AiContext context) => 1;

        public string Name => "Always one axis";
        public string Description => "Returns 1";
    }
}