// /** 
//  * BaseSingleton.cs
//  * Dylan Bailey
//  * 20161209
// */

#pragma warning disable 0414, 0219, 649, 169, 1570

namespace Zenobit.Common.Extensions
{
    #region Dependencies

    using FullInspector;
    using UnityEngine;
    using Zenobit.Common.Debug;

    #endregion

    /// <summary>
    ///     Be aware this will not prevent a non singleton constructor
    ///     such as `T myT = new T();`
    ///     To prevent that, add `protected T () {}` to your singleton class.
    ///     As a note, this is made as BaseBehavior because we need Coroutines.
    /// </summary>
    public class BaseSingleton<T> : BaseBehavior where T : BaseBehavior
    {
        private static T _instance;

        private static readonly object Lock = new object();

        private static bool _applicationIsQuitting;
        
        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting)
                {
                    //ZenLogger.LogWarning("[MonoSingleton] Instance '" + typeof(T) +
                    //	"' already destroyed on application quit." +
                    //	" Won't create again - returning null.");
                    return null;
                }

                lock (Lock)
                {
                    if (_instance != null) return _instance;

                    _instance = (T) FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        ZenLogger.LogError(
                            "[MonoSingleton] Something went really wrong " +
                            " - there should never be more than 1 singleton!" +
                            " Reopening the scene might fix it.");
                        return _instance;
                    }

                    if (_instance != null) return _instance;

                    var singleton = new GameObject();
                    _instance = singleton.AddComponent<T>();
                    singleton.name = "(singleton) " + typeof(T);

                    //DontDestroyOnLoad(singleton);

                    return _instance;
                }
            }
        }

        /// <summary>
        ///     When Unity quits, it destroys objects in a random order.
        ///     In principle, a MonoSingleton is only destroyed when application quits.
        ///     If any script calls Instance after it have been destroyed,
        ///     it will create a buggy ghost object that will stay on the Editor scene
        ///     even after stopping playing the Application. Really bad!
        ///     So, this was made to be sure we're not creating that buggy ghost object.
        /// </summary>
        public void OnDestroy()
        {
            _applicationIsQuitting = true;
        }
    }
}