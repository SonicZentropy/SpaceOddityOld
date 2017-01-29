// /** 
//  * IEcsSystem.cs
//  * Will Hart
//  * 20161205
// */

namespace Zenobit.Systems
{
    public interface IEcsSystem
    {
        bool Init();
        void Update();
        void FixedUpdate();
        void LateUpdate();
    }
}