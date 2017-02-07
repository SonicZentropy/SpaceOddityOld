// // /**
// //  * AIShipComp.cs
// //  * Dylan Bailey
// //  * 20170206
// // */
namespace Zen.Components
{
    using System.Collections.Generic;
    using UnityEngine;
    using Zen.AI.Common;
    using Zen.Common.ZenECS;

    public class AIShipComp : ComponentEcs
    {
        public class ShipNavigation
        {
            private Stack<EAINavState> AINavState = new Stack<EAINavState>();

            public void SetNavState(EAINavState newState)
            {
                if(AINavState.Count > 0)
                    AINavState.Pop();
                AINavState.Push(newState);
            }

            public void PushTempNavState(EAINavState tempState)
            {
                AINavState.Push(tempState);
            }

            public EAINavState GetNavState()
            {
                if (AINavState.Count <= 0)
                {
                    AINavState.Push(EAINavState.IDLE);
                }
                return AINavState.Peek();
            }

            public void RevertToPreviousNavState()
            {
                AINavState.Pop();
            }

            public bool HasReachedTarget { get; set; } = false;

            public Vector3 TargetPositionOffset;
        }
        public ShipNavigation Navigation = new ShipNavigation();

        public override ComponentTypes ComponentType => ComponentTypes.AIShipComp;
        public override string Grouping => "AI";
    }
}