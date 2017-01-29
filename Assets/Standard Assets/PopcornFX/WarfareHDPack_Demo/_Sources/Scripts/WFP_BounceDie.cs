using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WFP_BounceDie : MonoBehaviour {

	public GameObject		m_InstanciateOnDeath;
	public GameObject		m_InstanciateOnBounceBlood;
	public GameObject		m_InstanciateOnBounceGround;
	public GameObject		m_InstanciateOnBounceMetal;
	public GameObject		m_InstanciateOnBounceBag;
	public int				m_NbBounces;

	public List<PKFxFX> 	fxList;

	private int				m_CurrentNbBounces = 0;
	private ContactPoint	m_LastHit;

	void OnCollisionEnter(Collision col)
	{
		m_CurrentNbBounces++;
		m_LastHit = col.contacts[0];

		GameObject go;
		if (col.gameObject.tag == "ReactionGround" && m_InstanciateOnBounceGround != null)
		{
			go = GameObject.Instantiate(m_InstanciateOnBounceGround,
			                       m_LastHit.point + m_LastHit.normal.normalized/10.0f,
			                       //m_InstanciateOnBounce.transform.rotation);
			                       Quaternion.FromToRotation(Vector3.up, m_LastHit.normal)) as GameObject;
		}
		else if (col.gameObject.tag == "ReactionMetal" && m_InstanciateOnBounceMetal != null)
		{
			RaycastHit hi;
			bool hasRayHit = Physics.Raycast(m_LastHit.point + col.relativeVelocity/50.0f, -col.relativeVelocity.normalized, out hi);
			
			if (hasRayHit)
			{
				go = GameObject.Instantiate(m_InstanciateOnBounceMetal,
				                            hi.point + m_LastHit.normal.normalized/10.0f,
				                            //m_InstanciateOnBounce.transform.rotation);
				                            Quaternion.FromToRotation(Vector3.up, m_LastHit.normal)) as GameObject;
			}
			else
			{
				go = GameObject.Instantiate(m_InstanciateOnBounceMetal,
			                       m_LastHit.point + m_LastHit.normal.normalized/10.0f,
			                       //m_InstanciateOnBounce.transform.rotation);
			                            Quaternion.FromToRotation(Vector3.up, m_LastHit.normal)) as GameObject;
			}
		}
		else if (col.gameObject.tag == "ReactionBlood" && m_InstanciateOnBounceBlood != null)
		{
			RaycastHit hi;
			bool hasRayHit = Physics.Raycast(m_LastHit.point + col.relativeVelocity/50.0f, -col.relativeVelocity.normalized, out hi);

			if (hasRayHit)
			{
				go = GameObject.Instantiate(m_InstanciateOnBounceBlood,
				                            hi.point + m_LastHit.normal.normalized/10.0f,
				                            //m_InstanciateOnBounce.transform.rotation);
				                            Quaternion.FromToRotation(Vector3.up, m_LastHit.normal)) as GameObject;
			}
			else
			{
				go = GameObject.Instantiate(m_InstanciateOnBounceBlood,
					                       m_LastHit.point + m_LastHit.normal.normalized/10.0f,
				    	                   //m_InstanciateOnBounce.transform.rotation);
			            	                Quaternion.FromToRotation(Vector3.up, m_LastHit.normal)) as GameObject;
			}
		}
		else if (col.gameObject.tag == "ReactionBag" && m_InstanciateOnBounceBag != null)
		{
			RaycastHit hi;
			bool hasRayHit = Physics.Raycast(m_LastHit.point + col.relativeVelocity/50.0f, -col.relativeVelocity.normalized, out hi);
			
			if (hasRayHit)
			{
				go = GameObject.Instantiate(m_InstanciateOnBounceBag,
				                            hi.point + m_LastHit.normal.normalized/10.0f,
				                            //m_InstanciateOnBounce.transform.rotation);
				                            Quaternion.FromToRotation(Vector3.up, m_LastHit.normal)) as GameObject;
			}
			else
			{
				go = GameObject.Instantiate(m_InstanciateOnBounceBag,
				                            m_LastHit.point + m_LastHit.normal.normalized/10.0f,
				                            //m_InstanciateOnBounce.transform.rotation);
				                            Quaternion.FromToRotation(Vector3.up, m_LastHit.normal)) as GameObject;
			}
		}
		else
			go = null;
		if (go != null)
			go.transform.SetParent(col.gameObject.transform, true);

	}

	// Update is called once per frame
	void FixedUpdate () {
		if (m_CurrentNbBounces > m_NbBounces)
		{
			if (fxList.Count != 0) {
				foreach ( PKFxFX fx in fxList) {
					fx.StopEffect();
				}
			} else  {				
				PKFxFX fx = this.GetComponent<PKFxFX>();
				if (fx != null)
					fx.StopEffect();
			}

			if (m_InstanciateOnDeath != null)
			{
				GameObject.Instantiate(m_InstanciateOnDeath,
				                       m_LastHit.point,
				                       Quaternion.FromToRotation(Vector3.up, m_LastHit.normal));
			}
			GameObject.Destroy(this.gameObject);
		}
	}
}
