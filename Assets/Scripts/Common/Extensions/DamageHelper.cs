// /** 
//  * DamageHelper.cs
//  * Will Hart and Dylan Bailey
//  * 20161216
// */


namespace Zenobit.Common.Extensions
{
    #region Dependencies

    using Components;
    using UnityEngine;

    #endregion

    public static class DamageHelper
    {
        /// <summary>
        /// Calculates default damage on a single unit
        /// </summary>
        /// <param name="weapon"></param>
        /// <param name="enemy"></param>
        /// <returns></returns>
        public static float CalculateDefaultDamage(WeaponComp weapon, HealthComp enemy)
        {
            //var maxDamage = Mathf.Max(0, weapon.BaseDamage - enemy.ArmorValue) + weapon.PierceDamage;
            //return Mathf.CeilToInt(Random.Range(0.5f, 1f) * maxDamage);
	        return -987654321f;
        }
    }
}