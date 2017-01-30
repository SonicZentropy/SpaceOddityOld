﻿using System;
using UnityEngine;
using Zen.Common;
using Zen.Common.ZenECS;
using Zen.Components;

public class SelectionBox : ZenBehaviour, IOnAwake, IOnUpdate, IInitAfterECS
{
	public GameObject player;
	public float margin = 10;
	public GameObject selectionBoxSprite;

	private Vector3[] pts = new Vector3[8];
	public UISprite boxSprite;
	public TargetComp targetComp;
	private Rect r;

	private Camera cam;

	public void OnAwake() { boxSprite = GetComponent<UISprite>(); }

	void GetTargetComp()
	{
		var playerEnt = EcsEngine.Instance.FindEntity(Res.Entities.Player);
		targetComp = playerEnt.GetComponentDownward<TargetComp>();
	}

	public void InitAfterECS()
	{
		GetTargetComp();
		cam = Camera.main;
	}

	public void OnUpdate()
	{
		if (targetComp?.target == null)
		{
			boxSprite.enabled = false;
			return;
		}

		boxSprite.enabled = true;
		//followTarget.target = targetComp.target;
		//hudText.Add(2f, Color.yellow, 0f);
		// todo: optimize this
		Bounds b = targetComp.target.GetComponentInChildren<Renderer>().bounds;

		//The object is behind us
		if (cam.WorldToScreenPoint(b.center).z < 0) return;

		//All 8 vertices of the bounds
		pts[0] =
			cam.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z));
		pts[1] =
			cam.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z));
		pts[2] =
			cam.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z));
		pts[3] =
			cam.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z));
		pts[4] =
			cam.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z));
		pts[5] =
			cam.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z));
		pts[6] =
			cam.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z));
		pts[7] =
			cam.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z));

		//Get them in GUI space
		//for (int i = 0; i < pts.Length; i++) pts[i].y = Screen.height - pts[i].y;

		//Calculate the min and max positions
		Vector3 min = pts[0];
		Vector3 max = pts[0];
		for (int i = 1; i < pts.Length; i++)
		{
			min = Vector3.Min(min, pts[i]);
			max = Vector3.Max(max, pts[i]);
		}

		//Construct a rect of the min and max positions and apply some margin
		r = Rect.MinMaxRect(min.x, min.y, max.x, max.y);
		r.xMin -= margin;
		r.xMax += margin;
		r.yMin -= margin;
		r.yMax += margin;

		r.xMin -= Screen.width / 2f;
		r.xMax -= Screen.width / 2f;
		r.yMin -= Screen.height / 2f;
		r.yMax -= Screen.height / 2f;

		boxSprite.SetRect(r.xMin, r.yMin, r.width, r.height);
	}

	public override int ExecutionPriority { get; } = 0;
	public override Type ObjectType { get; } = typeof(SelectionBox);
}