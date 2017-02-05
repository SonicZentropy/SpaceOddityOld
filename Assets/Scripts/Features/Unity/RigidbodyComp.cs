// /** 
//  * RigidbodyComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zen.Components
{
    #region Dependencies

	using UnityEngine;
	using UnityEngine.Assertions;
	using Zen.Common.ZenECS;

    #endregion

    public class RigidbodyComp : ComponentEcs
    {
        public Rigidbody rigidbody;

	    public bool UseDefaults = true;

	    public float mass = 1f;
	    public float drag = 0f;
	    public float angularDrag = 0.05f;
	    public bool useGravity = false;
	    public bool isKinematic = false;

	    public RigidbodyInterpolation interpolation;
	    public CollisionDetectionMode collisionDetectionMode;
	    public RigidbodyConstraints constraints;
	    public float maxAngularVelocity = 7f;

		public override void InitialiseLate(EcsEngine _engine, Entity owner)
	    {
		    Assert.IsNotNull(rigidbody, "Rigidbody null in comp initialize");
		    if (UseDefaults) return;

		    rigidbody.mass = mass;
		    rigidbody.drag = drag;
		    rigidbody.angularDrag = angularDrag;
		    rigidbody.useGravity = useGravity;
		    rigidbody.isKinematic = isKinematic;
		    rigidbody.interpolation = interpolation;
		    rigidbody.collisionDetectionMode = collisionDetectionMode;
		    rigidbody.constraints = constraints;
		    rigidbody.maxAngularVelocity = maxAngularVelocity;
	    }

        public override ComponentTypes ComponentType => ComponentTypes.RigidbodyComp;
	    public override string Grouping => "Unity";
    }
}