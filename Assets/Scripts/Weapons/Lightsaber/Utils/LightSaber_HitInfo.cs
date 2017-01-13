// /** 
//  * LightSaberHitInfo.cs
//  * Dylan Bailey
//  * 20161104
// */

namespace Zenobit.Weapons.Lightsaber.Utils
{
    #region Dependencies

    using UnityEngine;

    #endregion

    public class LightSaberHitInfo
    {
        public LightSaber_Launcher launcher;
        public LightSaber_Launcher.RayInfo rayInfo;
        public RaycastHit raycastHit;
    }
}