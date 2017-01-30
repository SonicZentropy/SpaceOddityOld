// /**
//  * LauncherComp.cs
//  * Dylan Bailey
//  * 1/30/2017
// */

namespace Zen.Components
{
    #region Dependencies

    using UnityEngine;
    using Zen.Common.ZenECS;

    #endregion

    public class LauncherComp : ComponentEcs
    {
	    public Vector3 ProjectileLaunchOffset;

        public override ComponentTypes ComponentType => ComponentTypes.LauncherComp;
    }
}