// /** 
//  * AudioSourceComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zenobit.Components
{
    #region Dependencies

    using UnityEngine;
    using Zenobit.Common.Audio;
    using Zenobit.Common.ZenECS;

    #endregion

    public class AudioSourceComp : ComponentEcs
    {
        public AudioSource AudioSource;
        public override ComponentTypes ComponentType => ComponentTypes.AudioSourceComp;

        /// <summary>
        ///     Triggers a positional sound effect centered at this object's location
        /// </summary>
        /// <param name="sfx"></param>
        public void TriggerSfx(SfxTypes sfx)
        {
            AudioSource.PlayOneShot(Audioengine.Instance.SfxMapping[sfx]);
        }
    }
}