﻿/*using System;
using UnityEngine;
using Zenobit.Common.Extensions;
using Zenobit.Common.ZenECS;
using Zenobit.Components;

public class CollisionForwardingBehaviour : ZenBehaviour, IOnAwake
{
    public override Type ObjectType => typeof(CollisionForwardingBehaviour);
    public override int ExecutionPriority => 0;
	private CollisionEnterComp ceComp;

	public void OnAwake() { /*ceComp = GetComponent<EntityWrapper>().Entity.GetComponent<CollisionEnterComp>();#1# }

	public void OnCollisionEnter(Collision other)
	{
		ZenLogger.Log($"Adding collision to {other.gameObject.name}");
		//var ent = other.gameObject.GetComponent<EntityWrapper>().Entity;
		var ent = other.gameObject.GetEntity();
		//ent.GetComponent<CollisionEnterComp>().Other.Add(other);
		//ceComp.Other.Add(other);
	}
}*/