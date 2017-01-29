using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WFP_LODFx : MonoBehaviour
{
	public bool isLooping = false;
	public bool isRandColorAtt = false;
	public string nameColorAtt = "Color";

	[SerializeField]
	private List<PKFxFX> listLODFx = new List<PKFxFX>();

	// Update is called once per frame	
	void Update () {
	}

	public void ChangeLODFx(int n)
	{	
		if (isLooping) {
			StartFx(n);
		}
	}

	public void StartFx(int n)
	{
		StopFx();
		if  (isRandColorAtt)
			randomColor();
		listLODFx[n].StartEffect();
	}

	public void StopFx()
	{
		foreach (PKFxFX fx in listLODFx)
			fx.StopEffect();
	}

	public void randomColor()
	{
		Vector3 rand = new Vector3(Random.Range(0,1.0f), Random.Range(0,1.0f), Random.Range(0,1.0f));

		foreach (PKFxFX fx in listLODFx)
			fx.SetAttribute(new PKFxManager.Attribute(nameColorAtt,rand));
	}
}
