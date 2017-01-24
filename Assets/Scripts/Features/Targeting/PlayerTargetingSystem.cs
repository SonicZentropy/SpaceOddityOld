// /** 
// * PlayerTargetingSystem.cs
// * Will Hart and Dylan Bailey
// * 20161230
// */

namespace Zenobit.Systems
{
	#region Dependencies

	using System;
	using AdvancedInspector;
	using Common;
	using Common.Debug;
	using Common.Extensions;
	using Common.ZenECS;
	using Components;
	//using HighlightingSystem;
	using TypeSafe;
	using UniRx;
	using UnityEngine;

	#endregion

	public class PlayerTargetingSystem : AbstractEcsSystem
	{
		public Rect ScrRect;

		private Entity player;
		private Camera cam;
		private TargetComp targetComp;
		private int selectableLayerMask;
		private GameObject Target;
		//private Highlighter targetHighlighter;

		public override bool Init()
		{
			player = engine.FindEntity(Res.Entities.Player);
			cam = Camera.main;
			targetComp = player.GetComponent<ShipConnectionComp>().Ship.GetComponent<TargetComp>();
			selectableLayerMask = ZenUtils.LayerMaskFromIDs(SRLayerMask.npc, SRLayerMask.foreground);

			player.GetComponent<CommandComp>().SelectTarget.Where(x => x).Subscribe(SelectTargetClicked);
			return false;
		}

		private void SelectTargetClicked(bool reactive)
		{
			if (reactive)
			{
				RaycastClickTarget();
			}
		}

		private void RaycastClickTarget()
		{
			Ray mouseRay = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;

			if (Physics.Raycast(mouseRay, out hitInfo, float.MaxValue, selectableLayerMask))
			{
				//DisableHighlight();
				targetComp.target = hitInfo.transform;
				Target = hitInfo.transform.gameObject;
				//targetHighlighter = Target.GetComponent<Highlighter>();
				//if (targetHighlighter == null) targetHighlighter = Target.AddComponent<HighlighterOccluder>().gameObject.GetComponent<Highlighter>();
				//targetHighlighter.ConstantOn(Color.red);
				//Target.GetComponent<Highlighter>().SeeThroughOn();
				//ShowSelectionBox();
			}
		}

		
	
	}
}