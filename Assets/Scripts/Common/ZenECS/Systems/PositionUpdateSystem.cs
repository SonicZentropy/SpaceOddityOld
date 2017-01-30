// /** 
// * PositionUpdateSystem.cs
// * Dylan Bailey
// * 20161211
// */

namespace Zen.Systems
{
	#region Dependencies

	using Common.ZenECS;
	using Components;

	#endregion

	public class PositionUpdateSystem : AbstractEcsSystem
	{
		public override bool Init()
		{
			return true;
		}

		public override void Update()
		{
			UpdatePositions(false);
		}

		public override void LateUpdate()
		{
			UpdatePositions(true);
		}

		private void UpdatePositions(bool IsLateUpdate)
		{
			foreach (PositionComp position in engine.Get(ComponentTypes.PositionComp))
			{
				if (position.UseLateUpdate != IsLateUpdate) continue;
				position.Position = position.transform.position;
				position.Rotation = position.transform.rotation;
			}
		}
	}
}