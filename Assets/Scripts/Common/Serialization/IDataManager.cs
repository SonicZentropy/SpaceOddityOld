// /** 
//  * IDataengine.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zen.Common.Serialization
{
    public interface IDataengine
    {
        void SaveToJson<T>(T classToSave, string optionalInstanceId = "");
    }
}