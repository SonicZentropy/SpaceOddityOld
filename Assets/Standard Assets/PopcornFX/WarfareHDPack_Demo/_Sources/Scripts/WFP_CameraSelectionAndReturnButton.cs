using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WFP_CameraSelectionAndReturnButton : MonoBehaviour {
		
	public List<Camera> camList;
	public string mainMenuName = "MainMenu";

	public bool videoMode = false;

	void Awake () {
		if (PlayerPrefs.HasKey ("camera"))
			for (int i=0; i<camList.Count; i++)
				if (i != PlayerPrefs.GetInt ("camera")) {
					camList[i].gameObject.SetActive(false);
				} else {
					camList[i].gameObject.SetActive(true);
				}
	}
	
	// Update is called once per frame
	void Update () {

		// RETURN TO MAIN MENU
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.LoadLevel(mainMenuName);
		}
	}

	void OnGUI()
	{
		if (!videoMode)
			GUI.Label(new Rect(Screen.width-450, Screen.height-50, 800, 800), "<size=20>PRESS ESCAPE TO RETURN TO THE MENU</size>");
	}
}
