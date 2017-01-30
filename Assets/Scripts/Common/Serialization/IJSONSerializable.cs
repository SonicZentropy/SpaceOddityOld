// /** 
//  * IJSONSerializable.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zen.Common.Serialization
{
    #region Dependencies

    using System;
    using Zen.Common.ZenECS;

    #endregion

    public interface IJsonSerializable
    {
        string AssetName { get; }
        string AssetParentFolder { get; }

        Type ObjectType { get; }
        ComponentTypes ComponentType { get; }
        //void LoadFromJSON();
        //void SaveToJSON();
    }
}