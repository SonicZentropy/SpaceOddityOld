/*
// / ** 
//  * TargetSelectionSystem.cs
//  * Dylan Bailey
//  * 20161104
// * /

namespace Zen.Systems
{
    #region Dependencies

    using Common.ZenECS;
    using Components;
    using System;
    using System.Linq;

    #endregion

    /// <summary>
    /// Selects targets and weapons for mechs
    /// </summary>
    public class TargetSelectionSystem : AbstractEcsSystem
	{
        /// <summary>
        /// Loop through all living combat components and apply melee damage
        /// </summary>
        /// <param name="engine"></param>
        public override void Update()
        {
            var comps = engine.Get(ComponentTypes.UnitLoadoutComp);

            foreach (UnitLoadoutComp comp in comps)
            {
                UpdateTarget(comp);
                UpdateSelectedWeapon(comp);
            }
        }

        private static void UpdateSelectedWeapon(UnitLoadoutComp loadout)
        {
            var combat = loadout.Owner.GetComponent<CombatComp>();
            WeaponComp selectedWeapon = combat.SelectedWeapon;

            // TODO make weapon selection more intelligent especially where there are multiple weapons
            // TODO don't override player weapon selection made from the UI
            if (selectedWeapon == null)
            {
                combat.SelectedWeapon.Value = loadout.Weapons.Count == 0 ? null : WeaponLinkSystem.GetWeapon(loadout.Weapons[0]);
            }
        }

        private static void UpdateTarget(UnitLoadoutComp loadout)
        {
            // TODO handle player targeting separately
            var combat = loadout.Owner.GetComponent<CombatComp>();
            var posComp = loadout.Owner.GetComponent<PositionComp>();

            var nearest = EcsEngine.Instance.Get(ComponentTypes.PlayerComp)
                .Select(mech => mech.Owner.GetComponent<PositionComp>())
                .OrderBy(mechPos => (mechPos.Position - posComp.Position).sqrMagnitude)
                .FirstOrDefault();

            combat.TargetedEnemy = nearest;
        }
    }
}
*/
