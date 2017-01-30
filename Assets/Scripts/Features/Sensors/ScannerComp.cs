namespace Zen.Components
{
	using Common.ZenECS;

	public class ScannerComp : ComponentEcs
	{
		public float ScanRange;

		public override ComponentTypes ComponentType => ComponentTypes.ScannerComp;
	}
}