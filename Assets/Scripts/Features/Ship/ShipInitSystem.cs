// /** 
// * ShipInitSystem.cs
// * Will Hart and Dylan Bailey
// * 20161225
// */

namespace Zenobit.Systems
{
	#region Dependencies

	using System.Linq;
	using Common;
	using Common.ObjectPool;
	using Common.ZenECS;
	using Components;
	using UnityEngine;

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
			sdc.CurrentHull.Value = sdc.DefaultHull;
			sdc.CurrentShields.Value = sdc.DefaultShields;
			sdc.CurrentCargoSize = sdc.DefaultCargoSize;
			sdc.CurrentShieldRecharge = sdc.DefaultShieldRecharge;
			sdc.CurrentEnergy = sdc.DefaultEnergy;
			sdc.CurrentEnergyRecharge = sdc.DefaultEnergyRecharge;
		}

		private void FitWeapons(ShipComp sdc)
		{
			var sfc = sdc.GetComponent<ShipFittingsComp>();
			var awc = sdc.GetComponent<AvailableWeaponsComp>();

			var WeaponsGO = new GameObject("Weapons"); //Create game object on ship to hold weapons
			WeaponsGO.transform.SetParent(sdc.Owner.Wrapper.transform, false);

			foreach (var a in awc.AvailableWeapons)
			{
				a.Owner = sdc.Owner; //have to manually set owner of contained components
			}

			foreach (var fit in sfc.fittingList)
			{
				fit.WeaponFittingGO = new GameObject("WeaponFitting");
				Transform tf = fit.WeaponFittingGO.transform;
				tf.SetParent(WeaponsGO.transform, false);
				tf.localPosition = fit.PositionOffset;
				tf.localRotation = fit.RotationOffset;
				
				if (fit.FittedWeapon == null)
				{
					var avail = awc.AvailableWeapons.First(x => x.IsFitted == false);
					if (avail != null)
					{
						fit.FittedWeapon = avail;
						avail.IsFitted = true;
						avail.fittingAttached = fit;
						fit.IsEnabled = true;
						fit.WeaponTypesAllowed = avail.WeaponType;

						//spawn weapon prefab if exists
						if (!avail.WeaponPrefab.Contains("None"))
						{
							var weaponPF = Res.Load(avail.WeaponPrefab).InstantiateFromPool();
							weaponPF.SetActive(true);
							weaponPF.transform.SetParent(tf, false);
							avail.WeaponGameObject = weaponPF;
						}
					}
				}
			}
		}

		protected void AddShip(ComponentEcs comp)
		{
			if (comp.ComponentType == ComponentTypes.ShipComp)
				ApplyShipConfig((ShipComp)comp);
		}

		
	}
}

