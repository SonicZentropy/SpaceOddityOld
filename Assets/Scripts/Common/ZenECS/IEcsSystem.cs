// /** 
//  * IEcsSystem.cs
//  * Will Hart
//  * 20161205
// */

namespace Zenobit.Systems
{
    #region Dependencies

    using Zenobit.Common.ZenECS;

    #endregion

    public interface IEcsSystem
    {
        bool Init();
        void Update();
        void FixedUpdate();
        void LateUpdate();
    }
}