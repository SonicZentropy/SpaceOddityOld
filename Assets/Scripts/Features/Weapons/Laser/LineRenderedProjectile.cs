// /** 
//  * LineRenderedProjectile.cs
//  * Dylan Bailey
//  * 20161104
// */
#pragma warning disable 0618, 0649, 0414

namespace Zen.Weapons
{
    #region Dependencies

    using System.Collections.Generic;
    using MEC;
    using UnityEngine;
    using Zen.Components;

    using System;
    using Common.ZenECS;

	#endregion

    public class LineRenderedProjectile : ZenBehaviour, IOnStart
    {
        LineRenderer line;

        public int laserDistance = 10;
        public float laserSpeed = 2;
        public int laserDamage = 10;
        public int laserForce = 1;
        public float laserForceMult = 0.1f;

        public bool IsReleaseTimerRunning => true;

	    private CoroutineHandle DisableHandle = null;

        public void OnStart()
        {
            line = gameObject.AddComponent<LineRenderer>();
            line.enabled = false;
            throw new NotImplementedException();
            //var mat = Resourcesengine.MaterialsMapping[MaterialTypes.ThinArc];
            //line.material = mat;
        }

        public void FireProjectile(WeaponComp attacker, Transform target)
        {
            if (target == null) return;
            line.enabled = true;
            line.useWorldSpace = true;
            line.SetPosition(0, attacker.Owner.GetComponent<PositionComp>().Position);
            line.SetPosition(1, target.position);

            Timing.KillCoroutines(gameObject.GetInstanceID());
            Timing.RunCoroutine(DisableLaser(), gameObject.GetInstanceID());
        }

        IEnumerator<float> DisableLaser()
        {
            yield return Timing.WaitForSeconds(0.5f);
            if (line) line.enabled = false;
        }

        public void OnDisable()
        {
	        Timing.KillCoroutines(gameObject.GetInstanceID());
        }
		
	    public override void OnDestroy()
        {
	        Timing.KillCoroutines(gameObject.GetInstanceID());
	        base.OnDestroy();
        }

	    public override int ExecutionPriority { get; } = 0;
	    public override Type ObjectType { get; } = typeof(LineRenderedProjectile);
    }
}