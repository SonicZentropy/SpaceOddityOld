// /** 
//* PlayerMovementSystem.cs
//* Dylan Bailey
//* 20161214
//*/

namespace Zen.Systems
{
	#region Dependencies


	using UnityEngine;
	using Zen.Common;
	using Zen.Common.Extensions;
	using Zen.Common.ZenECS;
	using Zen.Components;

	#endregion

	public class PlayerMovementSystem : AbstractEcsSystem
	{
		private CommandComp cc;
		private PositionComp pc;
		private ShipComp sdc;
		private Rigidbody rbComp;
		private Camera cam;
		private Texture2D TargetingCursor;

		public float rotationDelta = 1.05f;

		public float smoothRotation = 1.5f;
		public float thrustModifier = 10f;
		public bool UseMouseDifference = true;
		private float trueSmooth;
		private float truePitch;
		private float trueRoll;
		private float trueYaw;

		private float trueThrust;

		private float invertYAxis;
		private Vector2 cursorMidpoint;
		private bool cursorIsTarget;
		private CursorMode cursorMode = CursorMode.ForceSoftware;

		private readonly Matcher _playerMatcher = new Matcher()
			.AllOf(ComponentTypes.PlayerComp);

		private bool SmoothMovements = false;

		public override bool Init()
		{
			//_command = _playerMatcher.GetSingleMatch().GetComponent<CommandComp>();
			cc = engine.FindEntity(Res.Entities.Player).GetComponent<CommandComp>();
			pc = cc.Owner.GetComponent<PositionComp>();
			rbComp = cc.Owner.GetComponent<RigidbodyComp>().rigidbody;
			sdc = cc.GetComponent<ShipComp>();
			cam = Camera.main;
			invertYAxis = engine.GetSingle<GameSettingsComp>(ComponentTypes.GameSettingsComp).InvertYAxis;
			TargetingCursor = Resources.Load<Texture2D>("Materials/TargetingCursor");
			cursorMidpoint = new Vector2(
				TargetingCursor.width / 2f,
				TargetingCursor.height / 2f);

			rbComp.maxAngularVelocity = sdc.CurrentMaxRotationVelocity;

			return true;
		}

		public override void Update()
		{
			HandleInertialDampers();

			float pitch = 0, yaw = 0;
			float currRotationSpeed = sdc.CurrentRotationSpeed;

			if (cc.MouseLookOn)
			{
				if (!cursorIsTarget)
				{
					Cursor.SetCursor(TargetingCursor, cursorMidpoint, cursorMode);
					cursorIsTarget = true;
				}
				if (UseMouseDifference)
				{
					var mousePos = cam.ScreenToViewportPoint(Input.mousePosition).Clamp(0, 1);
					mousePos -= new Vector3(0.5f, 0.5f, 0);
					mousePos *= 2f;
					pitch = -mousePos.y * currRotationSpeed;
					yaw = -mousePos.x * currRotationSpeed;
				}
				else
				{
					pitch = cc.MousePitchVertical * currRotationSpeed;
					yaw = cc.MouseRotateHorizontal * currRotationSpeed;
				}
			}
			else
			{
				if (cursorIsTarget)
				{
					Cursor.SetCursor(null, Vector2.zero, cursorMode);
					cursorIsTarget = false;
				}

				pitch = cc.PitchVertical * currRotationSpeed;
				yaw = cc.RotateHorizontal * currRotationSpeed;
			}

			var roll = cc.RollMovement * currRotationSpeed;
			//float rotMod = rotationDelta * Time.deltaTime;
			//pitch *= rotMod;
			//roll *= -rotMod;
			//yaw *= rotMod;

			if (SmoothMovements)
			{
				// Smothing Rotations...
				trueSmooth = Mathf.Lerp(trueSmooth, smoothRotation, 5 * Time.deltaTime);
				truePitch = Mathf.Lerp(truePitch, pitch, trueSmooth * Time.deltaTime);
				trueRoll = Mathf.Lerp(trueRoll, roll, trueSmooth * Time.deltaTime);
				trueYaw = Mathf.Lerp(trueYaw, yaw, trueSmooth * Time.deltaTime);
			}
			else
			{
				truePitch = pitch * 0.5f;
				trueRoll = -roll * 0.5f;
				trueYaw = yaw * 0.5f;
			}
			// * * This next block handles the thrust and drag.
			// This block sets the value of the joystick throttle ( float value between 0 and 1)
			var throttle = cc.Acceleration;

			if (throttle >= trueThrust)
			{
				trueThrust = Mathf.SmoothStep(trueThrust, throttle, sdc.CurrentAcceleration * Time.deltaTime);
			}
			if (throttle < trueThrust)
			{
				trueThrust = Mathf.Lerp(trueThrust, throttle, sdc.CurrentReverseAcceleration * Time.deltaTime);
			}
		}

		private void HandleInertialDampers()
		{
			if ((cc.InertialDampersOn && sdc.HasInertialDampers) || !cc.Acceleration.IsAlmost(0f))
			{
				rbComp.angularDrag = 1;
				rbComp.drag = 0;
			}
			else
			{
				rbComp.angularDrag = 5;
				rbComp.drag = 1;
			}
		}

		public override void FixedUpdate()
		{
			float velMag = rbComp.velocity.sqrMagnitude;

			if (cc.FullHalt)
			{
				rbComp.angularVelocity *= 0.975f;
				rbComp.velocity *= 0.975f;

				if (velMag <= 0.1f)
				{
					rbComp.velocity = Vector3.zero;
					rbComp.angularVelocity = Vector3.zero;
				}
			}
			else if (velMag <= sdc.CurrentMaxSpeed * sdc.CurrentMaxSpeed)
			{
				rbComp.AddRelativeForce(0, 0, trueThrust * thrustModifier);
			}

			rbComp.AddRelativeTorque(truePitch * Time.fixedDeltaTime, -trueYaw * Time.fixedDeltaTime, trueRoll * Time.fixedDeltaTime);

			
			//rbComp.MoveRotation(rbComp.rotation * Quaternion.Euler(truePitch*Time.fixedDeltaTime, -trueYaw * Time.fixedDeltaTime, trueRoll * Time.fixedDeltaTime));
		}

		public override void LateUpdate()
		{
			//pc.transform.Rotate(truePitch * Time.deltaTime, -trueYaw * Time.deltaTime, trueRoll * Time.deltaTime);
			
		}
	}
}