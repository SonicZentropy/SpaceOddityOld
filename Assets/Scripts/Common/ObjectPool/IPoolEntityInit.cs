// /** 
//  * IPoolEntityInit.cs
//  * Will Hart
//  * 20161218
// */

namespace Zen.Common.ObjectPool
{
    public interface IPoolEntityInit
    {
        /// Called by the PooledEntityInitSystem, a reactive system
        /// The interface needs to be accessible from the wrapping game object
        void InitEntityFromPool();

        void RemoveEntityFromPool();
    }
}