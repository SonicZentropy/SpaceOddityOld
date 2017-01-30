// /** 
//  * GuidLink.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zen.Common.ZenECS
{
    #region Dependencies

    using System;
    using UnityEngine;

    #endregion

    /// <summary>
    ///     Usage:
    ///     GuidLink<PositionComp> Position = new GuidLink<PositionComp>();
    ///     
    ///     Then access as normal
    /// </summary>
    [Serializable]
    public class GuidLink<T> where T : ComponentEcs
    {
        [SerializeField] private Guid _value;

        public GuidLink()
        {
            _value = Guid.Empty;
        }

        public GuidLink(T val)
        {
            _value = val.Id;
        }

        public Guid Id { get { return _value; } }

        public T Value
        {
            get { return EcsEngine.Instance.GetById<T>(_value); }
            set
            {
                _value = value.Id;
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator T(GuidLink<T> guidLink)
        {
            return guidLink.Value;
        }

        public static implicit operator GuidLink<T>(T obj)
        {
            return new GuidLink<T>(obj);
        }
    }
}