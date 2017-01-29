// /**
// * EcsEngineWrapper.cs
// * Will Hart and Dylan Bailey
// * 20161209
// */

namespace Zenobit.Common.ZenECS
{
	#region Dependencies

	using Systems;
	using AdvancedInspector;
	using UnityEngine;

	#endregion

	public class EcsEngineWrapper : MonoBehaviour
	{
		private EcsEngine _engine;

		[Inspect]public EcsEngine engine
		{
			get { return _engine ?? (_engine = EcsEngine.Instance); }
		}

		private void Awake()
		{
			EcsEngine.Instance.AddSystem(new GameInitSystem());
			EcsEngine.Instance.AddSystem(new DebugSystemZen());
		}

		private void Update()
		{
			EcsEngine.Instance.Update();
		}

		private void FixedUpdate()
		{
			EcsEngine.Instance.FixedUpdate();
		}

		private void LateUpdate()
		{
			EcsEngine.Instance.LateUpdate();
		}
	}
}