// /** 
//  * LightSaber_EmitterDestructor.cs
//  * Dylan Bailey
//  * 20161104
// */

namespace Zen.Weapons.Lightsaber.Utils
{
    using Common.ObjectPool;
    #region Dependencies

    using UnityEngine;

    #endregion

    public class LightSaber_EmitterDestructor : MonoBehaviour
    {

        public ParticleSystem partSystem;

        // Update is called once per frame
        void Update()
        {
            if (!partSystem.IsAlive())
                //Destroy(gameObject);
                gameObject.Release();
        }
    }
}
