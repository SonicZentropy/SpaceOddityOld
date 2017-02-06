// /** 
//  * TrackingCrosshairSystem.cs
//  * Dylan Bailey
//  * 20170205
// */


namespace Zen.Systems
{
    #region Dependencies

    using Features.Targeting;
    using Plugins.Zenobit;
    using UnityEngine;
    using Zen.Common;
    using Zen.Common.ZenECS;
    using Zen.Components;

    #endregion

    public class TrackingCrosshairSystem : AbstractEcsSystem
    {
        private Entity player;
        private PositionComp pc;
        private TargetComp tc;
        private ShipFittingsComp sfc;
        private Camera cam;
        private Camera UICam;
        private UIRoot uiRoot;
        private TrackingCrosshairComp crosshairComp;

        public override bool Init()
        {
            player = engine.GetPlayer();
            pc = player.GetComponent<PositionComp>();
            tc = player.GetComponent<TargetComp>();
            sfc = player.GetComponent<ShipFittingsComp>();
            crosshairComp = player.GetComponent<TrackingCrosshairComp>();
            cam = Camera.main;
            UICam = GameObject.Find("CameraUI").GetComponent<Camera>();
            uiRoot = GameObject.Find("UIRoot").GetComponent<UIRoot>();
            return true;
        }

        public override void Update()
        {
            if (tc.target == null) return;
            Vector3 playerToTarget = tc.target.position - pc.transform.position;
            float pttDistance = playerToTarget.magnitude;

            var mainWeapon = sfc.fittingList[0].FittedWeapon;
            float timeToTarget = pttDistance / mainWeapon.ProjectileSpeed;

            Rigidbody targetRB = tc.target.GetComponentInChildren<Rigidbody>();
            Vector3 estimatedPositionOfTarget = TargetPredictionHelper.PredictFuturePosition(tc.target, targetRB,
                                                                                             timeToTarget);
            
            Vector3 screenpos = cam.WorldToScreenPoint(estimatedPositionOfTarget);
            screenpos = new Vector3(screenpos.x - (Screen.width / 2f), screenpos.y - (Screen.height / 2f), 0);
            crosshairComp.crosshairSprite.transform.localPosition = new Vector3(screenpos.x, screenpos.y, 0);
        }

        
    }
}