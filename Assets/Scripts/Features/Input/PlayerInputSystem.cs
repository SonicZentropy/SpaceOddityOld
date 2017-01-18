// /** 
//  * PlayerInputSystem.cs
//  * Dylan Bailey
//  * 20161205
// */

namespace Zenobit.Systems
{
	#region Dependencies

	using System.Collections.Generic;
	using System.Linq;
	using Common;
	using Rewired;
	using UnityEngine;
	using Zenobit.Common.ZenECS;
	using Zenobit.Components;

	#endregion

	public class PlayerInputSystem : AbstractEcsSystem
	{
		private CommandComp _command;
		private Player player;
		private readonly Matcher _playerMatcher = new Matcher()
			.AllOf(ComponentTypes.PlayerComp);

		public override bool Init()
		{
			_command = _playerMatcher.GetSingleMatch().GetComponent<CommandComp>();
			player = ReInput.players.GetPlayer(0);
			return true;
		}

		public override void Update()
		{
			_command.PitchVertical = player.GetAxisRaw(RA.RotateVertical);
			_command.RotateHorizontal = player.GetAxisRaw(RA.RotateHorizontal);
			_command.MousePitchVertical = player.GetAxisRaw(RA.MouseRotateVertical);
			_command.MouseRotateHorizontal = player.GetAxisRaw(RA.MouseRotateHorizontal);
			_command.AttackPressed = player.GetButton(RA.Fire1);
			_command.Acceleration = player.GetAxisRaw(RA.Accelerate);
			
			_command.RollMovement = player.GetAxisRaw(RA.Roll);
			_command.InertialDampersOn = player.GetButton(RA.InertialDampers);
			_command.StrafeHorizontal = player.GetAxisRaw(RA.StrafeHorizontal);
			_command.StrafeVertical = player.GetAxisRaw(RA.StrafeVertical);
			_command.FullHalt = player.GetButton(RA.FullHalt);
			_command.SelectTarget.Value = player.GetButtonDown(RA.SelectTarget);

			if (player.GetButtonDown(RA.MouseLook))
			{
				_command.MouseLookOn = !_command.MouseLookOn;
				//Cursor.visible = !_command.MouseLookOn;
			}

		}
	}
}