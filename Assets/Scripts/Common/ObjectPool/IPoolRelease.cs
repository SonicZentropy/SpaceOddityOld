// /** 
//  * IPoolRelease.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zen.Common.ObjectPool
{
    internal interface IPoolRelease
    {
        /// This method prepares object for deactivation before it is released back into the pool
        void DeactivateBeforeRelease();
    }
}