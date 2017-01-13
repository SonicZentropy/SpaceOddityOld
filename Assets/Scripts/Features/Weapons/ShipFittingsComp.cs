namespace Zenobit.Components
{
	using System.Collections.Generic;
	using AdvancedInspector;
	using Common.ZenECS;
	using UnityEngine;

	public class ShipFittingsComp : ComponentEcs
	{
		[Inspect]public List<ShipFitting> fittingList = new List<ShipFitting>();

		public override ComponentTypes ComponentType => ComponentTypes.ShipFittingsComp;
	}


	public class ShipFitting
	{
		public ShipFitting()
		{
			PositionOffset = Vector3.zero;
			RotationOffset = Quaternion.identity;
		}

		[Inspect]public WeaponComp FittedWeapon;
		[Inspect] public bool IsEnabled;
		[Inspect]public Vector3 PositionOffset;
		[Inspect]public Quaternion RotationOffset;
		[Inspect] [ReadOnly] public GameObject WeaponFittingGO;

		[Inspect]public WeaponTypes WeaponTypesAllowed;
	}
}