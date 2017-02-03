namespace Common.Utility
{
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

}