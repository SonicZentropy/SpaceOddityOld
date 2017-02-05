namespace Common.Utility
{
	using UnityEngine;
	using System.Collections;

	public static class PhysicsHelper
	{

		public static void TorqueLookAtPoint(Rigidbody rigidbody, Vector3 point, float force, float damper = 0f)
		{
			Vector3 direction = point - rigidbody.position;
			TorqueLookToward(rigidbody, direction, force, damper);
		}


		public static void TorqueLookToward(Rigidbody rigidbody, Vector3 direction, float force, float damper = 0f)
		{

			Vector3 p = rigidbody.position;
			Vector3 forward = rigidbody.transform.forward; // axis we are rotating

			Vector3 cross = Vector3.Cross(forward, direction);

			float angleDiff = Vector3.Angle(forward, direction);
			angleDiff = Mathf.Sqrt(angleDiff);

			rigidbody.AddTorque(cross * angleDiff * force, ForceMode.Acceleration);
			rigidbody.AddTorque(-rigidbody.angularVelocity * damper, ForceMode.Acceleration);
			//rigidbody.AddTorque(cross * angleDiff * force);

			//Debug.Log(direction);
			//Debug.DrawLine(p, p + direction.normalized, Color.yellow, .05f);
			//Debug.DrawLine(p, p + rigidbody.angularVelocity, Color.yellow);
			//Debug.DrawLine(p, p + new Vector3(rigidbody.angularVelocity.x, 0, 0), Color.red);
			//Debug.DrawLine(p, p + new Vector3(0,rigidbody.angularVelocity.y, 0), Color.green);
			//Debug.DrawLine(p, p + new Vector3(0,0,rigidbody.angularVelocity.z), Color.blue);
		}

	}
}