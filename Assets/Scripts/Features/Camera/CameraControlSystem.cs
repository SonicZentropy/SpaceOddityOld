// /** 
// * CameraControlSystem.cs
// * Dylan Bailey
// * 20161212
// */

namespace Zen.Systems
{
	#region Dependencies

	using Common;
	using Common.ZenECS;
	using Components;
	using UnityEngine;

	#endregion

	public class CameraControlSystem : AbstractEcsSystem
	{
		private CameraComp cc;
		private Transform myTF;
		private Rigidbody camRB;
		private Rigidbody targetRB;

		private bool ParentCamera = true;

		public override bool Init()
		{
			cc = engine.GetSingle<CameraComp>(ComponentTypes.CameraComp);
			myTF = cc.MainCamera.transform;
			camRB = cc.MainCamera.gameObject.GetComponent<Rigidbody>();
			targetRB = cc.TargetToFollow.gameObject.GetComponentInChildren<Rigidbody>();

			if (ParentCamera)
			{
				ParentCameraToPlayer();
				return false;
			}
			return true;
		}

		private void ParentCameraToPlayer()
		{
			var player = engine.FindEntity(Res.Entities.Player);
			var camobj = cc.Owner.Wrapper.transform;
			camobj.SetParent(player.Wrapper.transform, false);
			camobj.localPosition = cc.StartingPositionOffset;
		}

		public override void Update()
		{
			if (cc.cameraUpdateTime == CameraUpdateTime.Update)
				UpdateCameraPosition(Time.deltaTime);
		}

		public override void FixedUpdate()
		{
			if (cc.cameraUpdateTime == CameraUpdateTime.FixedUpdate)
				UpdateCameraPosition(Time.fixedDeltaTime);
		}

		public override void LateUpdate()
		{
			if (cc.cameraUpdateTime == CameraUpdateTime.LateUpdate)
				UpdateCameraPosition(Time.deltaTime);
		}

		private void UpdateCameraPosition(float deltaTime)
		{
			
			//camRB.MovePosition(cc.TargetToFollow.transform.TransformPoint(cc.StartingPositionOffset));
			//camRB.MoveRotation(targetRB.rotation);


			myTF.position = cc.TargetToFollow.TransformPoint(cc.StartingPositionOffset);
			myTF.rotation = Quaternion.Slerp(myTF.rotation, cc.TargetToFollow.rotation, deltaTime / cc.RotationSpeed);
		}
	}
}