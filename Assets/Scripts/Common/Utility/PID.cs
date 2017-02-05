namespace Common.Utility.PID
{
	using System;
	using UnityEngine;
	using System.Collections;
	using Zen.Common.Extensions;

	[System.Serializable]
	public class PIDSingle {
		public float pFactor, iFactor, dFactor;

		float integral;
		float lastError;

		//present, past, future
		/// <summary>
		/// Basic PID Implementation. It works by measuring a numeric value and then
		/// comparing that against a desired value known as the setpoint. The difference
		/// between the two is called the error value, and the output of the controller
		/// is a correction value that should be used somehow to get the system back to the setpoint.
		///
		/// The PID concept (present, integral, derivative) essentially takes an estimate of the
		/// present, past and future of the system into account when deciding how to get it to track the setpoint.
		/// A good heuristic is that if you want your entity to move faster, increase kP. If you want its motion
		/// to be smoother, increase kD, and if you want it to be more relentless and robust, increase kI.
		/// </summary>
		/// <param name="presentFactor">How much the present state matters to the result</param>
		/// <param name="integralFactor">How much the past error matters to the result</param>
		/// <param name="derivativeFactor">How much the future state, disregarding the past and assuming nothing changes, matters to the result</param>
		public PIDSingle(float presentFactor, float integralFactor, float derivativeFactor) {
			pFactor = presentFactor;
			iFactor = integralFactor;
			dFactor = derivativeFactor;
		}

		/// <summary>
		/// Get the best-fit value update from the system.
		/// </summary>
		/// <param name="setpoint">The desired final target</param>
		/// <param name="actual">The current actual value</param>
		/// <param name="timeFrame">How long of a timeframe has passed since previous update (Time.deltaTime)</param>
		/// <returns>A best solution update value for the given single float value desired</returns>
		public float Update(float setpoint, float actual, float timeFrame) {
			float present = setpoint - actual;
			integral += present * timeFrame;
			float deriv = (present - lastError) / timeFrame;
			lastError = present;
			return present * pFactor + integral * iFactor + deriv * dFactor;
		}
	}


	public class ShipControlsPid : MonoBehaviour {

		public GUIText ScreenReadout;

		public Vector3 thrust = new Vector3(1,1,1);     //Total thrust per axis
		private Vector3 targetVelocity;                 //user input determines how fast user wants ship to rotate
		public Vector3 torques;                         //the amount of torque available for each axis, based on thrust
		public float maxRate = 4;                       //max desired turn rate
		private Vector3 curVelocity;                    //holds the rigidbody.angularVelocity converted from world space to local

		public Vector3 Kp = new Vector3(4, 4, 4);
		public Vector3 Ki = new Vector3(.007f,.007f,.007f);
		public Vector3 Kd = new Vector3(0,0,0);

		public new Rigidbody rigidbody;

		private PidController3Axis pControl = new PidController3Axis();

		void Start()
		{
			rigidbody = GetComponent<Rigidbody>();
			//this is where the bounding box is used to create pseudo-realistic torque;  If you want more detail, just ask.
			var shipExtents = ((MeshFilter)GetComponentInChildren(typeof(MeshFilter))).mesh.bounds.extents;
			torques.x = new Vector2(shipExtents.y,shipExtents.z).magnitude*thrust.x;
			torques.y = new Vector2(shipExtents.x,shipExtents.z).magnitude*thrust.y;    //normally would be x and z, but mesh is rotated 90 degrees in mine.
			torques.z = new Vector2(shipExtents.x,shipExtents.y).magnitude*thrust.z;    //normally would be x and y, but mesh is rotated 90 degrees in mine.

			ApplyValues();
		}

		void ApplyValues(){
			pControl.Kp = Kp;
			pControl.Ki = Ki;
			pControl.Kd = Kd;
			pControl.outputMax = torques;
			pControl.outputMin = torques * -1;
			pControl.SetBounds();
		}

		void RCS() {
			// Uncomment to catch inspector changes
			//ApplyValues();

			// collect inputs
			var rollInput = Input.GetAxisRaw("Roll");
			var pitchInput = Input.GetAxisRaw("Pitch");
			var yawInput = Input.GetAxisRaw("Yaw");

			//angular acceleration = torque/mass
			//var rates = torques/rigidbody.mass;

			//determine targer rates of rotation based on user input as a percentage of the maximum angular velocity
			targetVelocity = new Vector3(pitchInput*maxRate,yawInput*maxRate,rollInput*maxRate);

			//take the rigidbody.angularVelocity and convert it to local space; we need this for comparison to target rotation velocities
			curVelocity = transform.InverseTransformDirection(rigidbody.angularVelocity);

			// run the controller
			pControl.Cycle(curVelocity, targetVelocity, Time.fixedDeltaTime);
			rigidbody.AddRelativeTorque(pControl.output * Time.fixedDeltaTime, ForceMode.Impulse);

			if (ScreenReadout == null) return;
			ScreenReadout.text = "Current V : " + curVelocity + "\n"
			                     + "Target V :" + pControl.output + "\n";
			//+ "Current T : " + tActivation + "\n";

		}

		void FixedUpdate() {
			RCS();
		}

	}

	public class PidController3Axis {

		public Vector3 Kp;
		public Vector3 Ki;
		public Vector3 Kd;

		public Vector3 outputMax;
		public Vector3 outputMin;

		public Vector3 preError;

		public Vector3 integral;
		public Vector3 integralMax;
		public Vector3 integralMin;

		public Vector3 output;

		public void SetBounds(){
			integralMax = Divide(outputMax, Ki);
			integralMin = Divide(outputMin, Ki);
		}

		public Vector3 Divide(Vector3 a, Vector3 b){
			Func<float, float> inv = (n) => 1/(n.IsNotAlmost(0)  ? n : 1);
			var iVec = new Vector3(inv(b.x), inv(b.x), inv(b.z));
			return Vector3.Scale (a, iVec);
		}

		public Vector3 MinMax(Vector3 min, Vector3 max, Vector3 val){
			return Vector3.Min(Vector3.Max(min, val), max);
		}

		public Vector3 Cycle(Vector3 PV, Vector3 setpoint, float Dt){
			var error = setpoint - PV;
			integral = MinMax(integralMin, integralMax, integral + (error * Dt));

			var derivative = (error - preError) / Dt;
			output = Vector3.Scale(Kp,error) + Vector3.Scale(Ki,integral) + Vector3.Scale(Kd,derivative);
			output = MinMax(outputMin, outputMax, output);

			preError = error;
			return output;
		}
	}

}