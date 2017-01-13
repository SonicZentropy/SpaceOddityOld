// /** 
//  * MovementComp.cs
//  * Will Hart
//  * 20161103
// */

namespace Zenobit.Components
{
    #region Dependencies

	using AdvancedInspector;
	using Zenobit.Common.ZenECS;

    #endregion

    public class MovementComp : ComponentEcs
    {
        public float MoveSpeed {get; set;} = 5;
        public float CurrentMoveSpeed {get; set;}
        public MovementType MovementType  { get; set; } = MovementType.Ground;
		[Inspect]public bool UseFixedUpdateMovement { get; set; }

        public override ComponentTypes ComponentType => ComponentTypes.MovementComp;
    }

    public enum MovementType
    {
        Ground,
        Air
    }
}