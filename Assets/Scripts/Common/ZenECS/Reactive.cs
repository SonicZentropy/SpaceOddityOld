// /** 
//  * Reactive.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zenobit.Common.ZenECS
{
    #region Dependencies

    using System;
    using UnityEngine;

	#endregion

    /// <summary>
    ///     Usage:
    ///     Reactive<float> currHealth = new Reactive<float>(5);
    ///     currHealth.ValueUpdated += ValueUpdate;
    ///     void ValueUpdate(Reactive<float> val)
    ///     {
    ///         ZenLogger.Log("Updated: " + val);
    ///     }
    /// </summary>
    [Serializable]
    public class Reactive<T>
    {
        [SerializeField]
		private T _value;

        public Reactive(T val)
        {
            Value = val;
        }

        public T Value
        {
            get { return _value; }
            set
            {
                _value = value;
                ValueUpdated?.Invoke(this);
            }
        }

        //public event EventHandler ValueUpdated;
        public event Action<Reactive<T>> ValueUpdated;

        public void SetSilently(T val)
        {
            _value = val;
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public static implicit operator T(Reactive<T> reactive)
        {
            return reactive.Value;
        }

	    
    }
}