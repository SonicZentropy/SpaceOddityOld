// /** 
//  * ICustomInit.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zen.Common.ZenECS
{
    #region Dependencies

    using UnityEngine;

    #endregion

    public interface ICustomInit
    {
        void ExecuteInitialization(Entity e, GameObject go);
    }
}