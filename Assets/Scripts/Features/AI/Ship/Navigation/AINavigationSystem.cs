// /**
//  * AINavigationSystem.cs
//  * Dylan Bailey
//  * 2/2/2017
// */

namespace Zen.Systems
{
    #region Dependencies

    using Common.ZenECS;
    using Features.AI.Ship.Core;
    using UnityEngine;
    using Zen.AI.Common;
    using Zen.Common.Extensions;
    using Zen.Components;

    #endregion

    public class AINavigationSystem : AbstractEcsSystem
    {
        public float frequency = 0.13f;
        public float damping = 0.35f;

        public override bool Init()
        {
            return true;
        }

        public override void FixedUpdate()
        {
            foreach (var nav in engine.Get(ComponentTypes.AIShipComp))
            {
                var nc = (AIShipComp) nav;
                //var targ = nav.GetComponent<TargetComp>().target;
                switch (nc.Navigation.GetNavState())
                {
                    case EAINavState.ORBIT:
                        UpdateOrbit(nc);
                        break;
                    case EAINavState.APPROACH:
                        UpdateApproach(nc);
                        break;
                    case EAINavState.ATTACKING:
                        UpdateAttacking(nc);
                        break;
                    case EAINavState.IDLE:
                        return;
                    default:
                        Debug.Log("Default triggered!");
                        break;
                }
            }
        }

        private void UpdateAttacking(AIShipComp nc)
        {
            RotateTowardPosition(nc, nc.GetComponent<TargetComp>().target.position);
        }

        private void RotateTowardPosition(AIShipComp nc, Vector3 targetPosition)
        {
            var pc = nc.GetComponent<PositionComp>().transform;
            var moveVector = targetPosition - pc.position;

            pc.rotation =
                ZenMath.QuaternionUtil.SlerpLookAtTarget(pc, moveVector,
                                                         pc.GetComponent<ShipComp>().CurrentRotationSpeed *
                                                         Time.deltaTime);
        }

        private void UpdateApproach(AIShipComp nc)
        {
            MoveTowardPosition(nc, nc.GetComponent<TargetComp>().target.position);
        }

        private void UpdateOrbit(AIShipComp nc)
        {
            Vector3 moveto = nc.GetComponent<TargetComp>().target.position + nc.Navigation.TargetPositionOffset;
            MoveTowardPosition(nc, moveto);
        }

        private void MoveTowardPosition(AIShipComp nc, Vector3 moveToPosition)
        {
            //ZenGizmosDebug.Instance.targetPosition = moveToPosition;
            var pc = nc.GetComponent<PositionComp>().transform;
            var sc = nc.GetComponent<ShipComp>();
           
            pc.rotation =
                //ZenMath.QuaternionUtil.SlerpLookAtTarget(pc, moveVector, 5f /*Rotation speed*/* Time.deltaTime);
                ZenMath.QuaternionUtil.SlerpLookAtTarget(pc, moveToPosition, 0.5f /*Rotation speed*/* Time.deltaTime);

            //float faceAngle = Vector3.Dot(pc.forward, moveVector.normalized);
            Debug.DrawLine(pc.position, moveToPosition, Color.yellow);
            //if (faceAngle > 0.75) // only accelerate if pointing in the right general direction
            //{
            //    nc.GetComponent<RigidbodyComp>().rigidbody.AddForce(
            //                                                    pc.forward *
            //                                                    nc.GetComponent<ShipComp>().CurrentAcceleration  * 60f *
            //                                                    Time.deltaTime);
            //}
            var rb = nc.GetComponent<RigidbodyComp>().rigidbody;
            //calc force
            Vector3 force = CalculateForce(
                                           pc.position,
                                           rb.velocity,
                                           moveToPosition,
                                           Vector3.zero); // #TODO: Change this zero to a next waypoint direction velocity
            //rb.AddForce(force, ForceMode.VelocityChange);
            Vector3 v = (force / rb.mass) * Time.deltaTime * sc.CurrentAcceleration / 60f;
            Vector3 totalVelocity = rb.velocity + v;
            Vector3 limitedV = Vector3.ClampMagnitude(totalVelocity, sc.CurrentMaxSpeed);
            rb.velocity = limitedV;

            //float maxspeed = nc.GetComponent<ShipComp>().CurrentMaxSpeed;
            //getting close to target
            if ((pc.position - moveToPosition).sqrMagnitude < 50.1f)
            {
                //Debug.Log("Close to target");
                nc.Navigation.HasReachedTarget = true;
            }
            else
            {
                ZenLogger.LogGame($"Travel distance: {(pc.position - moveToPosition).sqrMagnitude}");
            }
        }

        //damping = 1, the system is critically damped
        //damping > 1 the system is over damped(sluggish)
        //damping is < 1 the system is under damped(it will oscillate a little)
        //Frequency is the speed of convergence.If damping is 1, frequency is the 1/time 
        //taken to reach ~95% of the target value.i.e.a frequency of 6 will bring you very 
        //close to your target within 1/6 seconds.
        private Vector3 CalculateForce(Vector3 currentPosition, Vector3 currentVelocity, Vector3 desiredPosition, Vector3 desiredVelocity)
        {
            // #TODO: factor everything down through ksg/kdg out to precalculate area or put in component
            float kp = (6f * frequency) * (6f * frequency) * 0.25f;
            float kd = 4.5f * frequency * damping;
            float dt = Time.fixedDeltaTime;
            float g = 1 / (1 + kd * dt + kp * dt * dt);
            float ksg = kp * g;
            float kdg = (kd + kp * dt) * g;
            Vector3 Pt0 = currentPosition;
            Vector3 Vt0 = currentVelocity;
            Vector3 F = (desiredPosition - Pt0) * ksg + (desiredVelocity - Vt0) * kdg;
            return F;
        }

        //public override void FixedUpdate()
        //{
        //	foreach (var nav in engine.Get(ComponentTypes.AIShipComp))
        //	{
        //		var nc = (AIShipComp) nav;
        //		var targ = nav.GetComponent<TargetComp>().target;
        //		if (nc.ShouldMove)
        //		{
        //			Vector3 moveto;
        //			if (targ != null)
        //			{
        //				moveto = targ.position + nc.TargetPositionOffset;
        //			}
        //			else
        //			{
        //				Debug.Log($"No target so moving to {nc.TargetPositionOffset}");
        //				moveto = nc.TargetPositionOffset;
        //			}
        //			var pc = nav.GetComponent<PositionComp>().transform;
        //			var move = moveto - pc.position;
        //			var rb = nav.GetComponent<RigidbodyComp>().rigidbody;
        //			//pc.Translate(move.normalized * 5f * Time.deltaTime, Space.World);
        //			rb.AddForce(move.normalized * 150f * Time.deltaTime);
        //
        //			//ZenGizmosDebug.Instance.targetPosition = moveto;
        //		}
        //	}
        //}
    }
}