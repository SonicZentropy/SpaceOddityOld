/****************
* BeginAttackAction.cs
* Dylan Bailey
* 2/4/2017
****************/

namespace Zen.AI.Apex.Actions
{
    #region Dependencies
	using global::Apex.AI;
	using Zen.AI.Apex.Contexts;
	#endregion

	public class BeginAttackAction : ActionBase<ShipContext>
	{
		public override void Execute(ShipContext context)
		{
			context.commandComp.AttackPressed = true;
		}
	}
}
