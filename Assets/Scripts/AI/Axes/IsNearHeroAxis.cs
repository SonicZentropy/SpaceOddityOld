// /** 
//  * IsNotNearHeroAxis.cs
//  * Will Hart
//  * 20161103
// */

namespace Zen.AI.Axes
{
    using Components;
    #region Dependencies

    using Zen.AI.Core;

    #endregion

    public class IsNearHeroAxis : IAxis
    {
        private readonly float _nearDistance;

        public IsNearHeroAxis(float nearDistance = 12)
        {
            _nearDistance = nearDistance;
        }

        public float Score(AiContext context)
        {
            PositionComp hero = context.GetComponent<CreepComp>().AssignedHero;
            if (hero == null) return 0;

            var pos = context.GetComponent<PositionComp>().Position;
            var sqrMag = (hero.Position - pos).sqrMagnitude;

            return Functions.Quartic(_nearDistance*_nearDistance/sqrMag);
        }

        public string Name => "Is Near Hero";
        public string Description => "Returns 1 when the position is near the hero, and returns 0 as the hero moves outside the passed nearDistance";
    }
}