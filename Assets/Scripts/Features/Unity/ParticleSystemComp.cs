// /** 
//  * ParticleSystemComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zenobit.Components
{
    #region Dependencies

    using UnityEngine;
    using Zenobit.Common.ZenECS;

    #endregion

    public class ParticleSystemComp : ComponentEcs
    {
        public ParticleSystem ParticleSystem;

        public override ComponentTypes ComponentType => ComponentTypes.ParticleSystemComp;
    }
}