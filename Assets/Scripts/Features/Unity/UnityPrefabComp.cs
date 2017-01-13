// /** 
//  * UnityPrefabComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zenobit.Components
{
    #region Dependencies

    using AdvancedInspector;
    using Zenobit.Common.ZenECS;

    #endregion

    public class UnityPrefabComp : ComponentEcs
    {
        [Inspect] [TextField(TextFieldType.Prefab)] public string PrefabLink = "Prefabs/None";

        [Inspect]
        public bool IsPooled { get; set; }

        public override ComponentTypes ComponentType => ComponentTypes.UnityPrefabComp;
    }
}