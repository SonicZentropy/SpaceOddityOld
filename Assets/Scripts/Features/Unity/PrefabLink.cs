using UnityEngine;
using Zen.Common;

public class PrefabLink
{
	private string prefabName;
	private GameObject _prefab;

	public GameObject Prefab
	{
		get
		{
			if (_prefab == null)
			{
				_prefab = Res.Load(prefabName);
			}
			return _prefab;
		}
	}

	public PrefabLink(string prefabToLink)
	{
		prefabName = prefabToLink;
		_prefab = Res.Load(prefabName);
	}
	//  User-defined conversion from double to Digit
	public static implicit operator PrefabLink(string prefabString)
	{
		return new PrefabLink(prefabString);
	}

	//  User-defined conversion from double to Digit
	public static implicit operator GameObject(PrefabLink p) { return p.Prefab; }
}
