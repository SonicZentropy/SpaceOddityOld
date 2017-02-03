namespace Zen.Components
{
	using AdvancedInspector;
	using Common.ZenECS;
	using UnityEngine;

	public class ShipComp : ComponentEcs
	{
		public Vector3 CameraOffset;

		public float DefaultRotationSpeed;
		public float DefaultMaxRotationVelocity;
		public float DefaultMaxSpeed;
		public float DefaultAcceleration;
		public float DefaultReverseAcceleration;
		public float DefaultCargoSize;
		public float DefaultShieldRecharge;
		public float DefaultEnergy;
		public float DefaultEnergyRecharge;

		private GuidLink<AbstractActorComp> _owningActor;

		[HideInInspector]
		public AbstractActorComp OwningActor
		{
			get { return _owningActor.Value; }
			set { _owningActor = value; }
		}

		[HideInInspector][ReadOnly]public bool HasInertialDampers;

		[ReadOnly]public float CurrentRotationSpeed;
		[ReadOnly]public float CurrentMaxRotationVelocity;
		[ReadOnly]public float CurrentMaxSpeed;
		[ReadOnly]public float CurrentAcceleration;
		[ReadOnly]public float CurrentReverseAcceleration;
		[ReadOnly]public float CurrentCargoSize;
		[ReadOnly]public float CurrentShieldRecharge;
		[ReadOnly]public float CurrentEnergy;
		[ReadOnly]public float CurrentEnergyRecharge;

		private void ReactToChanges(Reactive<float> reactive)
		{
			TriggerComponentUpdated(this);
		}

		public override ComponentTypes ComponentType => ComponentTypes.ShipComp;
		public override string Grouping => "Ship";
	}
}