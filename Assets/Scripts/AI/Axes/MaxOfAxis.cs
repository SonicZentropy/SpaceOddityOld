// /** 
//  * InvertAxis.cs
//  * Will Hart
//  * 20161110
// */

namespace Zenobit.AI.Axes
{
    #region Dependencies

    using Zenobit.AI.Core;
    using UnityEngine;

    #endregion

    public class MaxOfAxis : IAxis
    {
        private readonly IAxis _axis1;
        private readonly IAxis _axis2;

        public MaxOfAxis(IAxis axis1, IAxis axis2)
        {
            _axis1 = axis1;
            _axis2 = axis2;
        }

        public float Score(AiContext context)
        {
            var a = _axis1.Score(context);
            var b = _axis2.Score(context);
            return Mathf.Clamp01(Mathf.Max(a, b));
        }

        public override string ToString() => Name;
        public string Name => $"Max of {_axis1} and {_axis2}";
        public string Description => "Returns the larger score of the two enclosed axes";
    }
}