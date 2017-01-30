namespace Zen.Components
{
	using AdvancedInspector;
	using Common.ZenECS;
	using UniRx;

	public class CommandComp : ComponentEcs
	{
		[ReadOnly]public float PitchVertical { get; set; }
		[ReadOnly]public float RotateHorizontal { get; set; }
		[ReadOnly]
		public float MousePitchVertical { get; set; }
		[ReadOnly]
		public float MouseRotateHorizontal { get; set; }

		[ReadOnly]public float RollMovement { get; set; }
		[ReadOnly]public float StrafeVertical { get; set; }
		[ReadOnly]public float StrafeHorizontal { get; set; }
		[ReadOnly]public bool AttackPressed { get; set; }
		[ReadOnly]public bool InertialDampersOn { get; set; }
		[ReadOnly]public float Acceleration { get; set; }
		[ReadOnly]public bool FullHalt { get; set; }
		[ReadOnly]public bool MouseLookOn { get; set; }

		//[ReadOnly]public Reactive<bool> SelectTarget { get; set; } = new Reactive<bool>(false);

		public BoolReactiveProperty SelectTarget = new BoolReactiveProperty(false);
		
		public override ComponentTypes ComponentType => ComponentTypes.CommandComp;
	}
}