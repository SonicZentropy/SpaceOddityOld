namespace Zenobit.Common.ZenECS
{
	using System;
	using UnityEngine;

	public abstract class ZenBehaviour : MonoBehaviour, IZenBehaviour
	{
		public abstract int ExecutionPriority { get; }
		public abstract Type ObjectType { get; }

		public new Transform transform;
		public new GameObject gameObject;

		[HideInInspector]
		public virtual bool IsEnabled
		{
			get { return enabled; }
			set { enabled = value; }
		}

		public virtual bool IsActive
		{
			get { return enabled && gameObject.activeInHierarchy; }
		}

		private Guid _UniqueID;
		public Guid UniqueID => _UniqueID;

		public void Awake()
		{
			_UniqueID = Guid.NewGuid();
			transform = GetComponent<Transform>();
			gameObject = transform.gameObject;

			//Moved to awake so it's caught properly
			IsEnabled = enabled;
			EcsEngine.Instance.RegisterZenBehaviour(this);
		}

		//BUILD: Remove this, wasted cycles just so you can debug in inspector
		public void Start() { }

		public virtual void OnDestroy()
		{
			if (EcsEngine.Instance != null)
				EcsEngine.Instance.DeregisterZenBehaviour(this);
		}
	}
}