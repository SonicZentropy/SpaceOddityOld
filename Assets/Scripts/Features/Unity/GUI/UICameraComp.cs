// /** 
//  * UICameraComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zen.Components
{
    #region Dependencies

    using Zen.Common.ZenECS;

    #endregion

    public class UICameraComp : ComponentEcs
    {
        public UICamera UICamera;

        public override ComponentTypes ComponentType => ComponentTypes.UICameraComp;
	    public override string Grouping => "UnityGUI";
    }
}