// /** 
//  * IsFarFromTargetAxis.cs
//  * Will Hart
//  * 20161103
// */

namespace Zen.AI.Axes
{
    #region Dependencies

	using Core;
    using Components;
    using Common.ZenECS;

    #endregion

    public class IsFarFromTargetAxis : IAxis
    {
        private readonly float _nearDistance;

        public IsFarFromTargetAxis(float nearDistance = 10f)
        {
            _nearDistance = nearDistance;
        }

        public float Score(AiContext context)
        {
            var target = EcsEngine.Instance.GetById<HullComp>(context.State.AttackTargetHealth);

            if (target?.Owner == null) return 1;

            var targetPos = target.Owner.GetComponent<PositionComp>();
            var pos = context.GetComponent<PositionComp>().Position;
            var sqrMag = (targetPos.Position - pos).sqrMagnitude;

            return Functions.Octic(sqrMag/_nearDistance);
        }

        public string Name => "Is Not Near Hero";

        public string Description
            => "Returns 1 when the hero is far from the target, and returns 0 as the hero moves within nearDistance of the target";
    }
}