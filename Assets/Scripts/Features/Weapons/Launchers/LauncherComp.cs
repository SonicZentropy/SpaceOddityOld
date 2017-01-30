// /**
//  * LauncherComp.cs
//  * Dylan Bailey
//  * 1/30/2017
// */

namespace Zenobit.Components
{
    #region Dependencies

    using UnityEngine;
    using Zenobit.Common.ZenECS;

    #endregion

    public class LauncherComp : ComponentEcs
    {
	    public Vector3 ProjectileLaunchOffset;

        public override ComponentTypes ComponentType => ComponentTypes.LauncherComp;
    }
}