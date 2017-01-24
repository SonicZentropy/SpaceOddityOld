using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WFP_AmmoTrailBehaviour : MonoBehaviour {

	public float force = 1000;

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody>().AddForce(transform.right * force);
	}
	
	// Update is called once per frame
	void Update () {	
	}
}
