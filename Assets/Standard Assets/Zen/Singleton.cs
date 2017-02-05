// /** 
//  * Singleton.cs
//  * Dylan Bailey
//  * 20161210
// */

namespace Zen.Common.Extensions
{
    /// <summary>
    ///     Be aware this will not prevent a non singleton constructor
    ///     such as `T myT = new T();`
    ///     To prevent that, add `protected T () {}` to your singleton class.
    ///     As a note, this is made as MonoBehaviour because we need Coroutines.
    /// </summary>
    public class Singleton<T> where T : class, new()
    {
        private static T _instance;

        private static readonly object _lock = new object();

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ?? (_instance = new T());
                }
            }
        }
    }
}