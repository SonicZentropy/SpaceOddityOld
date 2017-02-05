﻿namespace Zen.Common.Debug
{
	using Rewired;
	using Zen.Common.Extensions;
	using Zen.Common.ZenECS;
	using Zen.Components;

	public class DebugInput : MonoSingleton<DebugInput>
	{
		private Entity player;
		private Player input;
		private readonly Matcher _playerMatcher = new Matcher()
			.AllOf(ComponentTypes.PlayerComp);

		void Awake()
		{
			input = ReInput.players.GetPlayer(0);
		}

		void Update()
		{
			if (player == null)
			{
				player = _playerMatcher.GetSingleMatch();
			}

			if (player == null) return;

			if (input.GetButtonDown(RA.DEBUG_LookAtTarget))
			{
				var tc = player.GetComponent<TargetComp>();

				if (tc.target == null) return;

				var pc = player.GetComponent<PositionComp>();
				pc.transform.LookAt(tc.target);
			}
		}
	}
}