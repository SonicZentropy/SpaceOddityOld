using System.Linq;
using UnityEngine;
using Zen.Common.ZenECS;
using Zen.Components;

public class DisableByDistanceInit : MonoBehaviour, ICustomInit
{
	public void ExecuteInitialization(Entity e, GameObject go)
	{
		var dbd = e.GetComponent<DisableByDistanceComp>();
		dbd.allBehaviours = gameObject.GetComponentsInChildren<MonoBehaviour>(true).ToList();
		dbd.allColliders =  gameObject.GetComponentsInChildren<Collider>(true).ToList();
		dbd.allParticles =  gameObject.GetComponentsInChildren<ParticleSystem>(true).ToList();
		dbd.allRenderers =  gameObject.GetComponentsInChildren<Renderer>(true).ToList();
	}
}