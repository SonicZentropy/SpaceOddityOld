namespace Zen.Common.Behaviours.CustomInit
{
	using AdvancedInspector;
	using UnityEngine;
	using Zen.Common.Extensions;

	public class EntityTagInitBehaviour : MonoBehaviour
	{
		[Enum(true)]
		public EntityTags entityTags;
		[Enum(true)]
		public Tags tags;

		void Awake()
		{
			gameObject.AddEntityTags(entityTags);
			gameObject.AddTags(tags);
			Destroy(this);
		}
	}
}