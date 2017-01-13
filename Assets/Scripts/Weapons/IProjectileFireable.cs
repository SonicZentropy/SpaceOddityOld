// /** 
//  * IProjectileFireable.cs
//  * Dylan Bailey
//  * 20161104
// */

namespace Zenobit.Weapons
{
    #region Dependencies
    
    using Zenobit.Components;

    #endregion

    public interface IProjectileFireable
	{
        /// <summary>
        /// Moves the projectile according to its own specific movement logic
        /// </summary>
        void Move();
        
        /// <summary>
        /// Gets a value indicating whether the projectile can be released back into the pool
        /// once the timer on ProjectileComp has expired
        /// </summary>
        /// <returns></returns>
        bool IsReleaseTimerRunning { get; }
    }
}