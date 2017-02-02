// /** 
//  * LightComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zen.Components
{
    #region Dependencies

    using UnityEngine;
    using Zen.Common.ZenECS;

    #endregion

    public class LightComp : ComponentEcs
    {
        public Light Light;

        public override ComponentTypes ComponentType => ComponentTypes.LightComp;
	    public override string Grouping => "Unity";
    }
}