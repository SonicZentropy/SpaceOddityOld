namespace AI.Core
{
	using Apex.AI;
	using Apex.Serialization;

	public abstract class ZenContextualScorer<T>
	 : ContextualScorerBase<T>
	where T : class, IAIContext
	{
		[ApexSerialization(defaultValue = false), FriendlyName("Invert", "Inversion bool")]
		public bool Invert;

		public float Success
		{
			get
			{
				if (Invert) return 0f;
				return score;
			}
		}

		public float Failure
		{
			get
			{
				if (Invert) return score;
				return 0f;
			}
		}
	}
}