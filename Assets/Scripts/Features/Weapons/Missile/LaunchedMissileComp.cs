namespace Zenobit.Components
{
	using AdvancedInspector;
	using Common.ZenECS;
	using UnityEngine;

	public class LaunchedMissileComp : ComponentEcs
	{
		[Inspect]
		[TextField(TextFieldType.Prefab)]
		public string ExplosionPrefabLink = "Prefabs/None";

		[HideInInspector] public GameObject ExplosionPrefab;

		[Tooltip("Method of flight behavior the missile uses. None = dumbfire")]
		public MissileHomingMethod homingMethod;
		[HideInInspector]public MissileInfoPacket projectileInfo;

		[Tooltip("Time (in seconds) before missile expires/self destructs")]
		public float lifeTime = 5f; // Missile life time
		[HideInInspector]public float TimeAlive;

		[ReadOnly]
		public Transform target;
		[HideInInspector]
		public LayerMask layerMask;

		[Tooltip("Distance from target (in Unity units) before explosion. If -1, rely on collision engine")]
		public float detonationDistance = 1;

		[Tooltip("Despawn delay for particle effect lifetimes")]
		public float despawnDelay; // Delay despawn in ms
		[Tooltip("Missile flight velocity")]
		public float velocity = 300f; // Missile velocity
		private float RaycastAdvance = 2f; // Raycast advance multiplier

		[Tooltip("Mostly for swarm missiles, causes a random dispersal pattern when fired")]
		public bool ShouldDisperse = true;
		private float dispersalTime = 2f;
		[SerializeField, Tooltip("The maximum rotation speed of the missile. In degrees per second. Its actual rotation may vary, because it's generated randomly.")]
		[HideInInspector]public float rotationSpeed = 10;

		#region ProNav Method

		Vector3 previousLos;
		Vector3 los;
		Vector3 losDelta;
		Vector3 desiredRotation;

		/// Gets or sets the navigational constant. The rotational change in the line of sight between the missile and the target is multiplied by this value. It should be greater than 1.
		/// Even though it is called a constant, feel free to change this at any point to get more control over the missile.
		[Tooltip("PID constant, should be greater than 1. Bigger = faster change")]
		public float navigationalConstant;

		private bool UseAdvancedMode = false; //use advanced PID calculation, probably not necessary

		#endregion

		#region SwarmMethod

		[HideInInspector] public float timeRandom;
		[HideInInspector] public float xRandom = 0.01f;
		[HideInInspector] public float yRandom = 0.01f;
		[HideInInspector] public float zRandom = 0.01f;
		[HideInInspector] public float swarmRandomRange = 0.1f;

		#endregion

		#region SwirlMethod

		//[Tooltip("Swirl Method")]
		//public float rocketTurnSpeed = 10f;
		[Tooltip("Swirl Method")]public float randomSwirlOffset = 50;

		#endregion

		[HideInInspector]public bool IsHit { get; set; } = false;

		[HideInInspector]public bool isFXSpawned = false; // Hit FX prefab spawned flag

		[HideInInspector]public Vector3 targetLastPos;
		[HideInInspector]public Vector3 step;

		[HideInInspector]public float explosionTime;

		[HideInInspector] public Collider myCollider, ownerCollider;

		public override ComponentTypes ComponentType => ComponentTypes.LaunchedMissileComp;
	}
}