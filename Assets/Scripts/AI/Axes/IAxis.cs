// /** 
//  * IAxis.cs
//  * Will Hart
//  * 20161103
// */

namespace Zenobit.AI.Axes
{
    #region Dependencies

    using Core;

    #endregion

    public interface IAxis
    {
        string Name { get; }
        string Description { get; }
        float Score(AiContext context);
    }
}