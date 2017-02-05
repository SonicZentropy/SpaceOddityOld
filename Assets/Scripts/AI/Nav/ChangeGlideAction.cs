/****************
* ChangeGlideAction.cs
* Dylan Bailey
* 2/4/2017
****************/

namespace Zen.AI.Apex.Actions
{
    #region Dependencies
	using global::Apex.AI;
	using global::Apex.Serialization;
	using Zen.AI.Apex.Contexts;
	using Zen.Components;

	#endregion

	public class ChangeGlideAction : ActionBase<ShipContext>
	{
		[ApexSerialization(defaultValue = true), FriendlyName("Enable", "Enables or disables glide")]
		public bool Enable;

	    [ApexSerialization(defaultValue = 0f), FriendlyName("GlideDrag", "Drag value used when glide enabled")]
	    public float GlideDrag;

	    [ApexSerialization(defaultValue = 1f),
	     FriendlyName("GlideAngularDrag", "Angular drag value used when glide enabled")]
	    public float GlideAngularDrag;

		public override void Execute(ShipContext context)
		{
			if (Enable)
			{
				context.rbComp.angularDrag = GlideAngularDrag;
				context.rbComp.drag = GlideDrag;
			}
			else
			{
				var rc = context.entity.GetComponent<RigidbodyComp>();
				context.rbComp.angularDrag = rc.angularDrag;
				context.rbComp.drag = rc.drag;
			}
		}
	}
}
