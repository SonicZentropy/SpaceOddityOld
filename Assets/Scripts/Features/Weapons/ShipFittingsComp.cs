namespace Zen.Components
{
	using System;
	using System.Collections.Generic;
	using AdvancedInspector;
	using Common.ZenECS;
	using UnityEngine;

	public class ShipFittingsComp : ComponentEcs
	{
		[Inspect]
		public List<ShipFitting> fittingList = new List<ShipFitting>();

		public override void Initialise(EcsEngine _engine, Entity owner)
		{
			base.Initialise(_engine, owner);
			foreach (var fitting in fittingList)
			{
				if (fitting.FittedWeapon != null)
				{
					fitting.FittedWeapon.fittingAttached = fitting;
				}
			}
		}

		public override ComponentTypes ComponentType => ComponentTypes.ShipFittingsComp;
	}

	[Serializable]
	public class ShipFitting
	{
		public ShipFitting()
		{
			PositionOffset = Vector3.zero;
			RotationOffset = Quaternion.identity;
		}

		[Inspect, CreateDerived]public WeaponComp FittedWeapon;
		[Inspect] public bool IsEnabled;
		[Inspect]public Vector3 PositionOffset;
		[Inspect]public Quaternion RotationOffset;
		[Inspect] [ReadOnly] public GameObject WeaponFittingGO;

		[Inspect, ReadOnly]
		public Vector3 ProjectileSpawnPositionOffset;

		[Inspect]public WeaponTypes WeaponTypesAllowed;
	}
}

//[CreateDerived]
//public List<WeaponComp> AvailableWeapons = new List<WeaponComp>();