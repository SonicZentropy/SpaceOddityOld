// /** 
//  * ObjectPoolId.cs
//  * Dylan Bailey
//  * 20161209
// */

#pragma warning disable 0414, 0219, 649, 169, 618, 1570

namespace Zenobit.Common.ObjectPool
{
    #region Dependencies

    using UnityEngine;

    #endregion
    

    public class ObjectPoolId : MonoBehaviour
    {
        public Transform MyParentTransform;
        public ObjectPool.Pool Pool { get; set; }
        public bool Free => IsFree;
        private bool IsFree { get; set; }
        public int ThisId => GetInstanceID();

        public int PrefabId
        {
            get
            {
                return MyParentTransform == null ? 0 : MyParentTransform.gameObject.GetInstanceID();
            }
        }

        public void SetFree(bool state)
        {
            IsFree = state;
        }

        public bool GetFree()
        {
            return IsFree;
        }
    }
}