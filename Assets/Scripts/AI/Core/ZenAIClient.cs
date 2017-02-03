namespace AI
{
	using System;
	using Apex.AI;
	using Apex.AI.Components;
	using Apex.LoadBalancing;
	using Apex.Serialization;
	using Zen.Common.Extensions;

	public class ZenAIClient : UtilityAIClient, ILoadBalanced
	{
		private ILoadBalancedHandle _lbHandle;

		/// <summary>
		/// Gets or sets the minimum execution interval in seconds.
		/// If <see cref="P:Apex.AI.Components.ZenAIClient.executionIntervalMin" /> and <see cref="P:Apex.AI.Components.ZenAIClient.executionIntervalMax" /> differ, the actual interval will be a random value in between the two (inclusive).
		/// </summary>
		public float executionIntervalMin { get; set; }

		/// <summary>
		/// Gets or sets the maximum execution interval in seconds.
		/// If <see cref="P:Apex.AI.Components.ZenAIClient.executionIntervalMin" /> and <see cref="P:Apex.AI.Components.ZenAIClient.executionIntervalMax" /> differ, the actual interval will be a random value in between the two (inclusive).
		/// </summary>
		public float executionIntervalMax { get; set; }

		/// <summary>
		/// Gets or sets the minimum number of seconds to delay the initial execution of the AI.
		/// If <see cref="P:Apex.AI.Components.ZenAIClient.startDelayMin" /> and <see cref="P:Apex.AI.Components.ZenAIClient.startDelayMax" /> differ, the actual delay will be a random value in between the two (inclusive).
		/// </summary>
		public float startDelayMin { get; set; }

		/// <summary>
		/// Gets or sets the maximum number of seconds to delay the initial execution of the AI.
		/// If <see cref="P:Apex.AI.Components.ZenAIClient.startDelayMin" /> and <see cref="P:Apex.AI.Components.ZenAIClient.startDelayMax" /> differ, the actual delay will be a random value in between the two (inclusive).
		/// </summary>
		public float startDelayMax { get; set; }

		bool ILoadBalanced.repeat => true;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Apex.AI.Components.ZenAIClient" /> class.
		/// </summary>
		/// <param name="aiId">The AI identifier.</param>
		/// <param name="contextProvider">The context provider.</param>
		/// <param name="executionIntervalMin">The minimum execution interval in seconds.</param>
		/// <param name="executionIntervalMax">The maximum execution interval in seconds.</param>
		/// <param name="startDelayMin">The minimum number of seconds to delay the initial execution of the AI.</param>
		/// <param name="startDelayMax">The maximum number of seconds to delay the initial execution of the AI.</param>
		public ZenAIClient(Guid aiId, IContextProvider contextProvider, float executionIntervalMin = 1f,
		                      float executionIntervalMax = 1f, float startDelayMin = 0.0f, float startDelayMax = 0.0f)
			: base(aiId, contextProvider)
		{
			this.executionIntervalMin = executionIntervalMin;
			this.executionIntervalMax = executionIntervalMax;
			this.startDelayMin = startDelayMin;
			this.startDelayMax = startDelayMax;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Apex.AI.Components.ZenAIClient" /> class.
		/// </summary>
		/// <param name="ai">The AI.</param>
		/// <param name="contextProvider">The context provider.</param>
		/// <param name="executionIntervalMin">The minimum execution interval in seconds.</param>
		/// <param name="executionIntervalMax">The maximum execution interval in seconds.</param>
		/// <param name="startDelayMin">The minimum number of seconds to delay the initial execution of the AI.</param>
		/// <param name="startDelayMax">The maximum number of seconds to delay the initial execution of the AI.</param>
		public ZenAIClient(IUtilityAI ai, IContextProvider contextProvider, float executionIntervalMin = 1f,
		                      float executionIntervalMax = 1f, float startDelayMin = 0.0f, float startDelayMax = 0.0f)
			: base(ai, contextProvider)
		{
			this.executionIntervalMin = executionIntervalMin;
			this.executionIntervalMax = executionIntervalMax;
			this.startDelayMin = startDelayMin;
			this.startDelayMax = startDelayMax;
		}

		/// <summary>Starts the AI.</summary>
		protected override void OnStart()
		{
			float minStartDelay = startDelayMin;
			if (minStartDelay.IsNotAlmost(startDelayMax))
				minStartDelay = UnityEngine.Random.Range(minStartDelay, startDelayMax);
			float executionInterval = executionIntervalMin;
			if (executionInterval.IsNotAlmost(executionIntervalMax))
				executionInterval = UnityEngine.Random.Range(executionInterval, executionIntervalMax);
			_lbHandle = AILoadBalancer.aiLoadBalancer.Add(this, executionInterval, minStartDelay);
		}

		/// <summary>
		/// Called after <see cref="M:Apex.AI.Components.IUtilityAIClient.Stop" />.
		/// </summary>
		protected override void OnStop()
		{
			if (_lbHandle == null)
				return;
			_lbHandle.Stop();
			_lbHandle = null;
		}

		/// <summary>
		/// Called after <see cref="M:Apex.AI.Components.IUtilityAIClient.Pause" />.
		/// </summary>
		protected override void OnPause()
		{
			_lbHandle?.Pause();
		}

		/// <summary>
		/// Called after <see cref="M:Apex.AI.Components.IUtilityAIClient.Resume" />.
		/// </summary>
		protected override void OnResume()
		{
			_lbHandle?.Resume();
		}

		float? ILoadBalanced.ExecuteUpdate(float deltaTime, float nextInterval)
		{
			Execute();
			if (executionIntervalMin.IsNotAlmost(executionIntervalMax))
				return UnityEngine.Random.Range(executionIntervalMin, executionIntervalMax);
			//return new float?();
			return executionIntervalMax;
		}

		// <summary>
		// Executes the AI. Typically this is called by whatever manager controls the AI execution cycle.
		// </summary>
		/*public void ExecuteThis()
		{
			IAIContext context = this._contextProvider.GetContext(this._ai.id);
			IAction action = this._ai.Select(context);
			bool flag = false;
			while (!flag)
			{
				ICompositeAction compositeAction = action as ICompositeAction;
				if (compositeAction == null)
				{
					IConnectorAction connectorAction = action as IConnectorAction;
					if (connectorAction == null)
						flag = true;
					else
						action = connectorAction.Select(context);
				}
				else
				{
					action.Execute(context);
					action = compositeAction.Select(context);
					flag = action == null;
				}
			}
			if (this._activeAction != null && !object.ReferenceEquals((object) this._activeAction, (object) action))
			{
				this._activeAction.Terminate(context);
				this._activeAction = action as IRequireTermination;
			}
			else if (this._activeAction == null)
				this._activeAction = action as IRequireTermination;
			if (action == null)
				return;
			action.Execute(context);
		}*/
	}
}