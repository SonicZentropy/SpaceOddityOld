﻿using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(FX_Input_Mgr))]
public class FX_Input_M_Editor : Editor {

	FX_Input_Mgr t;

    Color MyBlue = new Color(0f, 0.75f, 1f, 1f);

	public override void OnInspectorGUI(){
		t = (FX_Input_Mgr)target;
		EditorGUILayout.LabelField("Targeting Key Bindings", EditorStyles.boldLabel);
		RadarInput();
	}

    void OnEnable() {
        MarkDirty();
    }

    void MarkDirty() {
		 if(GameObject.Find("_GameMgr")) {
            Transform t = GameObject.Find("_GameMgr").transform;
            Undo.ClearUndo(t);
            t.position = Vector3.one;
            Undo.RecordObject (t, "Zero Transform Position");
            t.position = Vector3.zero;
        }
    }

	void RadarInput(){
        Header("Targeting Key Bindings");

        GUI.color = MyBlue;
		EditorGUILayout.LabelField("All Targets", EditorStyles.boldLabel);
        GUI.color = Color.white;
		t.TargetNext = (KeyCode)EditorGUILayout.EnumPopup ("Target Next:", t.TargetNext);
		t.TargetNextKM = (FX_Input_Mgr._keyMod)EditorGUILayout.EnumPopup ("Input Modifier:", t.TargetNextKM);
		EditorGUILayout.Space ();
		
		t.TargetPrev = (KeyCode)EditorGUILayout.EnumPopup ("Target Previous:", t.TargetPrev);
		t.TargetPrevKM = (FX_Input_Mgr._keyMod)EditorGUILayout.EnumPopup ("Input Modifier:", t.TargetPrevKM);
		EditorGUILayout.Space ();
		
		t.TargetClosest = (KeyCode)EditorGUILayout.EnumPopup ("Target Closest:", t.TargetClosest);
		t.TargetClosestKM = (FX_Input_Mgr._keyMod)EditorGUILayout.EnumPopup ("Input Modifier:", t.TargetClosestKM);
		EditorGUILayout.Space ();		
		
		EditorGUILayout.Space ();
        GUI.color = MyBlue;	
		EditorGUILayout.LabelField("Hostile Targets", EditorStyles.boldLabel);
        GUI.color = Color.white;
		t.TargetNextH = (KeyCode)EditorGUILayout.EnumPopup ("Target Next:", t.TargetNextH);
		t.TargetNextHKM = (FX_Input_Mgr._keyMod)EditorGUILayout.EnumPopup ("Input Modifier:", t.TargetNextHKM);
		EditorGUILayout.Space ();
		
		t.TargetPrevH = (KeyCode)EditorGUILayout.EnumPopup ("Target Previous:", t.TargetPrevH);
		t.TargetPrevHKM = (FX_Input_Mgr._keyMod)EditorGUILayout.EnumPopup ("Input Modifier:", t.TargetPrevHKM);
		EditorGUILayout.Space ();
		
		t.TargetClosestH = (KeyCode)EditorGUILayout.EnumPopup ("Target Closest:", t.TargetClosestH);
		t.TargetClosestHKM = (FX_Input_Mgr._keyMod)EditorGUILayout.EnumPopup ("Input Modifier:", t.TargetClosestHKM);
		EditorGUILayout.Space ();
		
       
     
		EditorGUILayout.Space ();
        GUI.color = MyBlue;	
		EditorGUILayout.LabelField("Sub Components", EditorStyles.boldLabel);
        GUI.color = Color.white;
        EditorGUILayout.Space ();	
        t.EnableSubComponents = EditorGUILayout.Toggle("SubComp Targeting", t.EnableSubComponents);
        EditorGUILayout.Space ();	

        if(t.EnableSubComponents) {
		    t.TargetNextS = (KeyCode)EditorGUILayout.EnumPopup ("Target Next:", t.TargetNextS);
		    t.TargetNextSKM = (FX_Input_Mgr._keyMod)EditorGUILayout.EnumPopup ("Input Modifier:", t.TargetNextSKM);
		    EditorGUILayout.Space ();
		
		    t.TargetPrevS = (KeyCode)EditorGUILayout.EnumPopup ("Target Previous:", t.TargetPrevS);
		    t.TargetPrevSKM = (FX_Input_Mgr._keyMod)EditorGUILayout.EnumPopup ("Input Modifier:", t.TargetPrevSKM);
		    EditorGUILayout.Space ();

		    EditorGUILayout.Space ();
		    t.ClearSubC = (KeyCode)EditorGUILayout.EnumPopup ("Clear Sub Comp:", t.ClearSubC);
		    t.ClearSubCKM = (FX_Input_Mgr._keyMod)EditorGUILayout.EnumPopup ("Input Modifier:", t.ClearSubCKM);
        }

        EditorGUILayout.Space ();	
        GUI.color = MyBlue;
		EditorGUILayout.LabelField("Target Clearing", EditorStyles.boldLabel);
        GUI.color = Color.white;
		t.ClearTarget = (KeyCode)EditorGUILayout.EnumPopup ("Clear Target:", t.ClearTarget);
		t.ClearTargetKM = (FX_Input_Mgr._keyMod)EditorGUILayout.EnumPopup ("Input Modifier:", t.ClearTargetKM);

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();	
        GUI.color = MyBlue;
		EditorGUILayout.LabelField("Store Selected Target", EditorStyles.boldLabel);
        GUI.color = Color.white;
		t.StoreTarget1 = (KeyCode)EditorGUILayout.EnumPopup ("Store Target 1:", t.StoreTarget1);
		t.StoreTarget1KM = (FX_Input_Mgr._keyMod)EditorGUILayout.EnumPopup ("Input Modifier:", t.StoreTarget1KM);
		EditorGUILayout.Space ();

		t.StoreTarget2 = (KeyCode)EditorGUILayout.EnumPopup ("Store Target 2:", t.StoreTarget2);
		t.StoreTarget2KM = (FX_Input_Mgr._keyMod)EditorGUILayout.EnumPopup ("Input Modifier:", t.StoreTarget2KM);
		EditorGUILayout.Space ();

		t.StoreTarget3 = (KeyCode)EditorGUILayout.EnumPopup ("Store Target 3:", t.StoreTarget3);
		t.StoreTarget3KM = (FX_Input_Mgr._keyMod)EditorGUILayout.EnumPopup ("Input Modifier:", t.StoreTarget3KM);
		EditorGUILayout.Space ();

		t.StoreTarget4 = (KeyCode)EditorGUILayout.EnumPopup ("Store Target 4:", t.StoreTarget4);
		t.StoreTarget4KM = (FX_Input_Mgr._keyMod)EditorGUILayout.EnumPopup ("Input Modifier:", t.StoreTarget4KM);
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

        GUI.color = MyBlue;
		EditorGUILayout.LabelField("Recall Stored Target", EditorStyles.boldLabel);
        GUI.color = Color.white;
		t.SelectStoredTarget1 = (KeyCode)EditorGUILayout.EnumPopup ("Stored Target 1:", t.SelectStoredTarget1);
		t.SelectStoredTarget1KM = (FX_Input_Mgr._keyMod)EditorGUILayout.EnumPopup ("Input Modifier:", t.SelectStoredTarget1KM);
		EditorGUILayout.Space ();
		
		t.SelectStoredTarget2 = (KeyCode)EditorGUILayout.EnumPopup ("Stored Target 2:", t.SelectStoredTarget2);
		t.SelectStoredTarget2KM = (FX_Input_Mgr._keyMod)EditorGUILayout.EnumPopup ("Input Modifier:", t.SelectStoredTarget2KM);
		EditorGUILayout.Space ();
		
		t.SelectStoredTarget3 = (KeyCode)EditorGUILayout.EnumPopup ("Stored Target 3:", t.SelectStoredTarget3);
		t.SelectStoredTarget3KM = (FX_Input_Mgr._keyMod)EditorGUILayout.EnumPopup ("Input Modifier:", t.SelectStoredTarget3KM);
		EditorGUILayout.Space ();
		
		t.SelectStoredTarget4 = (KeyCode)EditorGUILayout.EnumPopup ("Stored Target 4:", t.SelectStoredTarget4);
		t.SelectStoredTarget4KM = (FX_Input_Mgr._keyMod)EditorGUILayout.EnumPopup ("Input Modifier:", t.SelectStoredTarget4KM);
        Footer();
		
		EditorGUILayout.Space ();
        Header("Targeting List & Filters Key Bindings");

		//t.NAVList = (KeyCode)EditorGUILayout.EnumPopup ("Display NAV List:", t.NAVList);
		//t.NAVListKM = (FX_Input_Mgr._keyMod)EditorGUILayout.EnumPopup ("Input Modifier:", t.NAVListKM);
		//EditorGUILayout.Space ();
		
		t.TargetList = (KeyCode)EditorGUILayout.EnumPopup ("Display Target List:", t.TargetList);
		t.TargetListKM = (FX_Input_Mgr._keyMod)EditorGUILayout.EnumPopup ("Input Modifier:", t.TargetListKM);
		EditorGUILayout.Space ();
		
		t.FilterHostile = (KeyCode)EditorGUILayout.EnumPopup ("Filter Hostile:", t.FilterHostile);
		t.FilterHostileKM = (FX_Input_Mgr._keyMod)EditorGUILayout.EnumPopup ("Input Modifier:", t.FilterHostileKM);

        Footer();
		
        EditorGUILayout.Space ();
		Header("Display Key Bindings");
        GUI.color = MyBlue;
		EditorGUILayout.LabelField("Radar 3D/2D", EditorStyles.boldLabel);
        GUI.color = Color.white;
		t.Switch3D2D = (KeyCode)EditorGUILayout.EnumPopup ("Switch 3D/2D:", t.Switch3D2D);
		t.Switch3D2DKM = (FX_Input_Mgr._keyMod)EditorGUILayout.EnumPopup ("Input Modifier:", t.Switch3D2DKM);

        EditorGUILayout.Space ();
		
        GUI.color = MyBlue;
		EditorGUILayout.LabelField("Radar Zoom", EditorStyles.boldLabel);
        GUI.color = Color.white;
		t.ZoomNormal = (KeyCode)EditorGUILayout.EnumPopup ("Zoom Normal", t.ZoomNormal);
		t.ZoomNormalKM = (FX_Input_Mgr._keyMod)EditorGUILayout.EnumPopup ("Input Modifier:", t.ZoomNormalKM);
		EditorGUILayout.Space ();
		t.ZoomIn = (KeyCode)EditorGUILayout.EnumPopup ("Zoom In", t.ZoomIn);
		t.ZoomInKM = (FX_Input_Mgr._keyMod)EditorGUILayout.EnumPopup ("Input Modifier:", t.ZoomInKM);
		EditorGUILayout.Space ();
		t.ZoomOut = (KeyCode)EditorGUILayout.EnumPopup ("Zoom Out", t.ZoomOut);
		t.ZoomOutKM = (FX_Input_Mgr._keyMod)EditorGUILayout.EnumPopup ("Input Modifier:", t.ZoomOutKM);
		EditorGUILayout.Space ();

		//t.ToggleIndicators = (KeyCode)EditorGUILayout.EnumPopup ("Display Indicators:", t.ToggleIndicators);
		//t.ToggleIndicatorsKM = (FX_Input_Mgr._keyMod)EditorGUILayout.EnumPopup ("Input Modifier:", t.ToggleIndicatorsKM);
		
		Footer();
	}

    void Header(string s) {
		EditorGUI.indentLevel = 0;
		EditorGUILayout.Space ();
        GUI.color = new Color(1f, 0.75f, 0f, 1f);
		EditorGUILayout.LabelField(s, EditorStyles.boldLabel);
        GUI.color = Color.gray;
		EditorGUILayout.BeginVertical ("HelpBox", GUILayout.MaxWidth(245),GUILayout.MinWidth(245));
		EditorGUILayout.Space ();
        GUI.color = Color.white;
    }

    void Footer() {
		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical();
    }

}