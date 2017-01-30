// /** 
//  * InvertAxis.cs
//  * Will Hart
//  * 20161110
// */

namespace Zen.AI.Axes
{
    #region Dependencies

    using Zen.AI.Core;
    using UnityEngine;

    #endregion

    public class InvertAxis : IAxis
    {
        private readonly IAxis _axis;

        public InvertAxis(IAxis axis)
        {
            _axis = axis;
        }

        public float Score(AiContext context)
        {
            return Mathf.Clamp01(1 - _axis.Score(context));
        }

        public override string ToString()
        {
            return $"{Name} {_axis}";
        }

        public string Name => "Invert";
        public string Description => "Returns 1 minus the value of the enclosed axis";
    }
}