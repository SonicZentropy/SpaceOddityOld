/****************
* BasicNavigationContext.cs
* Dylan Bailey
* 1/31/2017
****************/

namespace Zen.AI.Apex.Contexts
{
	using System.Collections.Generic;
	using global::Apex.AI;
	using UnityEngine;

	public class BasicNavigationContext  : IAIContext
    {
	    public GameObject self { get; private set; }
	    public Rigidbody rbComp { get; private set; }

	    public List<GameObject> observations;

	    public BasicNavigationContext(GameObject gameObject)
	    {
		    self = gameObject;
		    rbComp = gameObject.GetComponentInChildren<Rigidbody>();
		    observations = new List<GameObject>();
	    }
    }
} 
