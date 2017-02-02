// /** 
//  * AudioSourceComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zen.Components
{
    #region Dependencies

    using UnityEngine;
    using Zen.Common.Audio;
    using Zen.Common.ZenECS;

    #endregion

    public class AudioSourceComp : ComponentEcs
    {
        public AudioSource AudioSource;
        public override ComponentTypes ComponentType => ComponentTypes.AudioSourceComp;
	    public override string Grouping => "Unity";

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