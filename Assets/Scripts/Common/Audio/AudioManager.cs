// /** 
//  * Audioengine.cs
//  * Dylan Bailey
//  * 20161209
// */


#pragma warning disable 0414, 0219, 649, 169, 618, 1570

namespace Zenobit.Common.Audio
{
    #region Dependencies

    using System.Collections.Generic;
    using UnityEngine;
    using Zenobit.Common.Extensions;

    #endregion

    public class Audioengine : MonoSingleton<Audioengine>
    {
        [SerializeField] private AudioSource _audioSource2D;
        public Dictionary<SfxTypes, AudioClip> SfxMapping;

        public void OnAwake()
        {
            SfxMapping = new Dictionary<SfxTypes, AudioClip>
            {
                {SfxTypes.GunFire, Resources.Load("Audio/Sfx/Gunfire") as AudioClip}
            };

            _audioSource2D = gameObject.AddComponent<AudioSource>();
            _audioSource2D.loop = false;
            _audioSource2D.playOnAwake = false;
            _audioSource2D.spatialBlend = 0.0f;
        }

        /// <summary>
        ///     Plays a one shot 2D sound effect.  This has no spatial processing at all
        /// </summary>
        /// <param name="sfx"></param>
        public void Trigger2DSfx(SfxTypes sfx)
        {
            _audioSource2D.PlayOneShot(SfxMapping[sfx]);
        }
    }

    public enum SfxTypes
    {
        GunFire,
        Explosion,
        UnitProduced
    }
}