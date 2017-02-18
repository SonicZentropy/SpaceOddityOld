// /** 
// * PlayerTargetingSystem.cs
// * Dylan Bailey
// * 20161230
// */

namespace Zen.Systems
{
	#region Dependencies

	using Common;
	using Common.Extensions;
	using Common.ZenECS;
	using Components;
	//using HighlightingSystem;
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
	    //private GameObject radarRIDGO;
	    //private FX_3DRadar_RID radarRID;

		public override bool Init()
		{
			player = engine.FindEntity(Res.Entities.Player);
			cam = Camera.main;
			targetComp = player.GetComponent<TargetComp>();
			selectableLayerMask = ZenUtils.LayerMaskFromIDs(SRLayerMask.npc, SRLayerMask.foreground);

			player.GetComponent<CommandComp>().SelectTarget.Where(x => x).Subscribe(SelectTargetClicked);
		    //radarRIDGO = GameObject.Find("Target_1");
		    //radarRID = radarRIDGO.GetComponent<FX_3DRadar_RID>();
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
				//Target = hitInfo.transform.gameObject;
			    //radarRID = Target.GetComponentInChildren<FX_3DRadar_RID>();
			    //radarRID.ThisRigidbody = Target.GetComponentInChildren<Rigidbody>();
                //radarRID.ThisRenderer.Add(Target.GetComponentInChildren<Renderer>());
			    //radarRID.ThisButton = Target.GetComponentInChildren<RectTransform>();
			}
		}

		
	
	}
}