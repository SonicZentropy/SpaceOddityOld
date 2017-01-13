namespace Zenobit.Components
{
	using System.Collections.Generic;
	using AdvancedInspector;
	using Common.ZenECS;
	using UnityEngine;

	public class DisableByDistanceComp : ComponentEcs
	{
		[Inspect]
		public bool IsActive { get; set; }

		[Inspect] public bool DisableRenderers { get; set; }
		[Inspect]
		public bool DisableScripts { get; set; }
		[Inspect]
		public bool DisableParticles { get; set; }
		[Inspect]
		public bool DisableCollisions { get; set; }

		public List<Renderer> allRenderers = new List<Renderer>();
		public List<MonoBehaviour> allBehaviours = new List<MonoBehaviour>();
		public List<ParticleSystem> allParticles = new List<ParticleSystem>();
		public List<Collider> allColliders = new List<Collider>();

		public override ComponentTypes ComponentType => ComponentTypes.DisableByDistanceComp;
	}
}