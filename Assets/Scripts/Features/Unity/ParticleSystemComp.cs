// /** 
//  * ParticleSystemComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zen.Components
{
    #region Dependencies

    using UnityEngine;
    using Zen.Common.ZenECS;

    #endregion

    public class ParticleSystemComp : ComponentEcs
    {
        public ParticleSystem ParticleSystem;

        public override ComponentTypes ComponentType => ComponentTypes.ParticleSystemComp;
    }
}