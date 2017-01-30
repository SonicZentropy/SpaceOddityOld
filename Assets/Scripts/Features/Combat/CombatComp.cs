﻿ /** 
  * CombatComp.cs
  * Dylan Bailey
  * 20161103
 */

namespace Zen.Components
{
    #region Dependencies

	using FullInspector;
    using Zen.Common.ZenECS;

    #endregion

    public class CombatComp : ComponentEcs
    {
        //[ShowInInspector]
        //public GuidLink<WeaponComp> SelectedWeapon { get; set; } = new GuidLink<WeaponComp>();

        [ShowInInspector]
        public GuidLink<PositionComp> TargetedEnemy { get; set; } = new GuidLink<PositionComp>();

        //public float MeleeDamageMultiplier = 1;
        //public float RangedDamageMultiplier = 1;

        //public Dictionary<WeaponTypes, float> NextAttackTime = new Dictionary<WeaponTypes, float>();
	    //public float NextAttackFloat;
                
        public override ComponentTypes ComponentType => ComponentTypes.CombatComp;
    }
}