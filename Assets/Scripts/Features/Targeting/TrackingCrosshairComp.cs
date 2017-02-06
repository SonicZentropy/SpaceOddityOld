// /** 
//  * TrackingCrosshairComp.cs
//  * Dylan Bailey
//  * 20170205
// */

namespace Zen.Components
{
    #region Dependencies

    using UnityEngine;
    using Zen.Common.ZenECS;

    #endregion

    public class TrackingCrosshairComp : ComponentEcs
    {
        public GameObject crosshairSprite;

        public override void Initialise(EcsEngine _engine, Entity owner)
        {
            base.Initialise(_engine, owner);
            crosshairSprite = GameObject.Find("TrackingCrosshairNGUI");
        }

        public override ComponentTypes ComponentType => ComponentTypes.TrackingCrosshairComp;
    }
}