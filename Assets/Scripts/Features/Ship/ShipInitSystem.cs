// /** 
// * ShipInitSystem.cs
// * Dylan Bailey
// * 20161225
// */

namespace Zen.Systems
{
	#region Dependencies

	using System.Linq;
	using Common;
	using Common.ZenECS;
	using Components;
	using UnityEngine;
	using Zen.Common.Extensions;

	#endregion

	public class ShipInitSystem : AbstractEcsSystem
	{
		public override bool Init()
		{
			engine.OnComponentAdded += AddShip;
			ApplyAllShipsConfig();

			return false;
		}

		public virtual void Dispose()
		{
			engine.OnComponentAdded -= AddShip;
		}

		protected void ApplyAllShipsConfig()
		{
			foreach (ShipComp comp in engine.Get(ComponentTypes.ShipComp))
			{
				ApplyShipConfig(comp);
			}
		}

		protected void ApplyShipConfig(ShipComp sdc)
		{
			InitializeCurrentShipValues(sdc);
			FitWeapons(sdc);
		}

		private void InitializeCurrentShipValues(ShipComp sdc)
		{
			sdc.CurrentAcceleration = sdc.DefaultAcceleration;
			sdc.CurrentMaxSpeed = sdc.DefaultMaxSpeed;
			sdc.CurrentReverseAcceleration = sdc.DefaultReverseAcceleration;
			sdc.CurrentRotationSpeed = sdc.DefaultRotationSpeed;
			sdc.CurrentMaxRotationVelocity = sdc.DefaultMaxRotationVelocity;
			sdc.CurrentCargoSize = sdc.DefaultCargoSize;
			sdc.CurrentShieldRecharge = sdc.DefaultShieldRecharge;
			sdc.CurrentEnergy = sdc.DefaultEnergy;
			sdc.CurrentEnergyRecharge = sdc.DefaultEnergyRecharge;
		}

		private void FitWeapons(ShipComp sdc)
		{
			var sfc = sdc.GetComponent<ShipFittingsComp>();
			//var awc = sdc.GetComponent<AvailableWeaponsComp>();

			var WeaponsGO = new GameObject("Weapons"); //Create game object on ship to hold weapons
			WeaponsGO.transform.SetParent(sdc.Owner.Wrapper.transform, false);

			foreach (var fit in sfc.fittingList.Where(x => x.FittedWeapon != null))
			{
				InitFitting(sdc, fit, WeaponsGO);
			}
		}

		private static void InitFitting(ShipComp sdc, ShipFitting fit, GameObject WeaponsGO)
		{
			fit.FittedWeapon.Owner = sdc.Owner; //have to manually set owner of contained components
			fit.WeaponFittingGO = new GameObject("WeaponFitting");
			Transform tf = fit.WeaponFittingGO.transform;
			tf.SetParent(WeaponsGO.transform, false);
			tf.localPosition = fit.PositionOffset;
			tf.localRotation = fit.RotationOffset;

			//spawn weapon prefab if exists
			if (fit.FittedWeapon.LauncherPrefab.DoesNotContain("None"))
			{
				var weaponPF = Res.CreateFromPool(fit.FittedWeapon.LauncherPrefab);
				weaponPF.SetActive(true);
				weaponPF.transform.SetParent(tf, false);
				//weaponPF.transform.localPosition = fit.PositionOffset;
				fit.ProjectileSpawnPositionOffset = weaponPF.GetComponent<LauncherBehaviour>()?.LaunchPosOffset ?? Vector3.zero;
			}
		}

		protected void AddShip(ComponentEcs comp)
		{
			if (comp.ComponentType == ComponentTypes.ShipComp)
				ApplyShipConfig((ShipComp) comp);
		}
	}
}