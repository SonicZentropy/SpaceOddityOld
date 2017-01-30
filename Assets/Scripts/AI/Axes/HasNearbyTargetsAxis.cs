// /** 
//  * HasNearbyTargetsAxis.cs
//  * Will Hart
//  * 20161103
// */

namespace Zen.AI.Axes
{
    #region Dependencies

	using Zen.AI.Core;

	#endregion

    public class HasNearbyTargetsAxis : IAxis
    {
        public float Score(AiContext context)
        {
            //WeaponComp selectedWeapon = context.GetComponent<CombatComp>().SelectedWeapon;
            //PositionComp targetedEnemy = context.GetComponent<CombatComp>().TargetedEnemy;
            //if (selectedWeapon == null || targetedEnemy == null) return 0;
			//
            //var pos = context.GetComponent<PositionComp>().Position;
            //var range = (pos - targetedEnemy.Position).sqrMagnitude / (selectedWeapon.AttackRange * selectedWeapon.AttackRange);
			//
            //return Mathf.Clamp01(1f - range);
	        return 0f; //need to refactor above for combatcomp
        }

        public string Name => "Has Nearby Targets";
        public string Description => "Returns zero if no targets are in range, otherwise returns a positive number";
    }
}