/****************
* BasicNavigationContextProvider.cs
* Dylan Bailey
* 1/31/2017
****************/

namespace Zen.AI.Apex.Contexts
{
	using System;
	using AdvancedInspector;
	using global::Apex.AI;
	using global::Apex.AI.Components;
	using UnityEngine;

	public class BasicNavigationContextProvider  : MonoBehaviour, IContextProvider
	{
		[Inspect]private IAIContext _context;

		private void Awake()
		{
			_context = new BasicNavigationContext(gameObject);
		}

	    public IAIContext GetContext(Guid aiId)
	    {
		    return _context;
	    }
    }
} 
