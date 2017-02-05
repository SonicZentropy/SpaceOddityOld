// /**
// * EcsEngineWrapper.cs
// * Dylan Bailey
// * 20161209
// */

namespace Zen.Common.ZenECS
{
	#region Dependencies

	using Systems;
	using AdvancedInspector;
	using FullInspector;
	using UnityEngine;

	#endregion

    [fiInspectorOnly]
	public class EcsEngineWrapper : MonoBehaviour
    {
        public string wrapperName = "Wrapper";
		[SerializeField]
        private EcsEngine _engine;

		//[Inspect]
        public EcsEngine engine
		{
			get { return _engine ?? (_engine = EcsEngine.Instance); }
		}

		protected void Awake()
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