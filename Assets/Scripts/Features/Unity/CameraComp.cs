// /** 
//  * CameraComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zen.Components
{
    #region Dependencies

	using AdvancedInspector;
	using UnityEngine;
    using Zen.Common.ZenECS;

    #endregion

    public class CameraComp : ComponentEcs
    {
		[HideInInspector]
        public Camera MainCamera;
		[Inspect]
		public CameraUpdateTime cameraUpdateTime { get; set; }
		[Inspect]
		public CameraType cameraType { get; set; } = CameraType.FirstPerson;
		[Inspect]public bool UseManualCamera { get; set; }

		[Inspect]
	    public float MoveSpeed { get; set; }

		[Inspect]
		public float ZoomSpeed { get; set; }
		
		[Inspect]
	    public float RotationSpeed { get; set; } = 0.001f;

		[ReadOnly]
		public Transform TargetToFollow;

	    private Vector3 start;

	    public Vector3 StartingPositionOffset
	    {
		    get { return start; }
		    set
		    {
			    start = value;
		    }
	    }
	    public Vector3 StartingRotationOffset;
		
		public override ComponentTypes ComponentType => ComponentTypes.CameraComp;
	    public override string Grouping => "Unity";
    }

	public enum CameraType
	{
		FirstPerson,
		ThirdPersonChase
	}

	public enum CameraUpdateTime
	{
		Update,
		FixedUpdate,
		LateUpdate
	}
}