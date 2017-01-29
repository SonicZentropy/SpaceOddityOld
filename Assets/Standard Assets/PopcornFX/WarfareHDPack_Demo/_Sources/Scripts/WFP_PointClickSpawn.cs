using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WFP_PointClickSpawn : MonoBehaviour
{	
	public float rotateSpeed = 10;
	private float tempRotateSpeed;
	
	public int maxPlayableFx = 5;

	public List<GameObject>	m_FX;
	public Texture 			tex;
	public Texture 			tex2;
	public Texture 			tex3;
	public Texture 			tex4;

	private int				m_CurrentFx = 0;
	private List<GameObject> listMaxFX = new List<GameObject>();

	private int currentLOD;
	public int LODCount = 3; // there are 3 LOD (HD,SD,LD)

	void Awake()
	{
		Cursor.visible = true;

		tempRotateSpeed = rotateSpeed;

		currentLOD = LODCount - 1;

		// The number of FX can't be < 0
		if (maxPlayableFx <= 0)
			maxPlayableFx = 1;
	}

	void OnGUI()
	{
		// HUD : Arrows : L R
		if (tex != null) {
			GUI.DrawTexture (new Rect (10, 0, tex.width/2.0f, tex.height/2.0f), tex);
			GUI.Label (new Rect (120, 0+50, tex.width, tex.height/2.0f), "Switch Fx");
		}
		// HUD : Arrows : U D
		if (tex2 != null) {
			GUI.DrawTexture(new Rect(10, 80, tex2.width/2.0f, tex2.height/2.0f), tex2);
			GUI.Label (new Rect (120, 80+50, tex2.width, tex2.height/2.0f), "Switch the Level Of Detail");
		}
		// HUD : Stop Camera
		if (tex2 != null) {
			GUI.DrawTexture(new Rect(10, 160, tex3.width/2.0f, tex3.height/2.0f), tex3);
			GUI.Label (new Rect (120, 160+50, tex3.width, tex3.height/2.0f), "Stop the Camera");
		}
		// HUD : Mouse Click
		if (tex4 != null) {
			GUI.DrawTexture(new Rect(15, 250, tex4.width/2.0f, tex4.height/2.0f), tex4);
			GUI.Label (new Rect (120, 250+50, tex4.width, tex4.height/2.0f), "Play the Fx");
		}

		// HUD : FX Name
		GUI.Label(new Rect(Screen.width-400, 50, 400, 400), "<size=30>FX : "+(m_CurrentFx+1)+" / "+m_FX.Count+"\nName : "+m_FX[m_CurrentFx].name+"\nLOD : "+getLODName(currentLOD)+"</size>");
	}

	void Update () {

		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			StopAllFx();
			m_CurrentFx = (m_CurrentFx + 1) % m_FX.Count;
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			StopAllFx();
			m_CurrentFx = (m_CurrentFx - 1) % m_FX.Count;
			if (m_CurrentFx < 0)
				m_CurrentFx = m_FX.Count + m_CurrentFx;
		}

		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit = new RaycastHit ();
			if (Physics.Raycast (ray, out hit, 500)) {
				GameObject prefab = m_FX [m_CurrentFx];
				GameObject go = Instantiate (prefab, hit.point + hit.normal.normalized / 10.0f, prefab.transform.rotation) as GameObject;
				go.transform.Rotate (Quaternion.FromToRotation (Vector3.up, hit.normal).eulerAngles);
				go.transform.Translate (prefab.transform.position);				
								
				listMaxFX.Add(go);
				listMaxFX[listMaxFX.Count-1].GetComponent<WFP_LODFx>().StartFx(currentLOD);

				if (listMaxFX.Count > maxPlayableFx) {					
					if (listMaxFX[0] != null) {
						listMaxFX[0].GetComponent<WFP_LODFx>().StopFx();
						Destroy(listMaxFX[0]);
						listMaxFX.RemoveAt(0);
					}
				}	
			}
		}

		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			currentLOD = (currentLOD + 1) % LODCount;
			for (int i=0; i<listMaxFX.Count; i++) 
				if (listMaxFX[i] != null)
					listMaxFX[i].GetComponent<WFP_LODFx>().ChangeLODFx(currentLOD);
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			currentLOD = (currentLOD - 1) % LODCount;
			if (currentLOD < 0)
				currentLOD = LODCount + currentLOD;

			for (int i=0; i<listMaxFX.Count; i++)
				if (listMaxFX[i] != null)
					listMaxFX[i].GetComponent<WFP_LODFx>().ChangeLODFx(currentLOD);
		}

		// STOP CAMERA
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (tempRotateSpeed > 0)
				tempRotateSpeed = 0;
			else
				tempRotateSpeed = rotateSpeed;
		}
		
		// Rotation of the Camera
		transform.Rotate(Vector3.up * Time.deltaTime * tempRotateSpeed, Space.World);
	}

	void StopAllFx()
	{
		foreach (GameObject fx in listMaxFX) {
			if (fx != null) {
				fx.GetComponent<WFP_LODFx>().StopFx();
				Destroy(fx);
			}
		}
		listMaxFX.Clear();
	}

	public int getCurrentLOD() {
		return currentLOD;
	}
	
	public string getLODName(int n)
	{
		string r = "HD";

		if (n == 0)
			r = "LD";
		else if (n == 1)
			r = "SD";

		return r;
	}
}
