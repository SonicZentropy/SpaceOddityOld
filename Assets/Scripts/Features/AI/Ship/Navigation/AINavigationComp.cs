// /**
//  * AINavigationComp.cs
//  * Dylan Bailey
//  * 2/2/2017
// */

namespace Zen.Components
{
    #region Dependencies

    using AdvancedInspector;
    using UnityEngine;
    using Zen.Common.Debug;
    using Zen.Common.ZenECS;

	#endregion

    public class AINavigationComp : ComponentEcs
    {
        private EAINavState _AINavState = EAINavState.IDLE;
        [Inspect, Enum(false, EnumDisplay.DropDown)]
        public EAINavState AINavState
        {
            get { return _AINavState; }
            set
            {
                _AINavState = value;
                InGameConsole.Instance.SetAIState(value);
            }
        } 

        public bool HasReachedTarget { get; set; } = false;

        public Vector3 TargetPositionOffset;

        public override ComponentTypes ComponentType => ComponentTypes.AINavigationComp;
	    public override string Grouping => "AI";
    }

    public enum EAINavState
    {
        IDLE,
        PURSUE,
        FLEE,
        ORBIT,
        APPROACH,
        ATTACKING,
        AVOIDANCE
    }
}