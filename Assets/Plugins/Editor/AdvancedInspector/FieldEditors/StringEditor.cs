using UnityEngine;
using UnityEditor;
using System;

namespace AdvancedInspector
{
	using System.Collections.Generic;

	public class StringEditor : FieldEditor
    {
        public override Type[] EditedTypes
        {
            get { return new Type[] { typeof(string) }; }
        }

        public override void Draw(InspectorField field, GUIStyle style)
        {
            GUILayout.BeginHorizontal();

            float width = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 0;

            TextFieldAttribute text = field.GetAttribute<TextFieldAttribute>();
            MultilineAttribute multiline = field.GetAttribute<MultilineAttribute>();
            TextAreaAttribute area = field.GetAttribute<TextAreaAttribute>();

            object value = GetValue(field);

            EditorGUI.BeginChangeCheck();
            GUIUtility.GetControlID(field.Path.GetHashCode(), FocusType.Passive);

            string result = "";
            if ((text == null && multiline == null && area == null) || (text != null && text.Type == TextFieldType.Standard))
            {
                if (style != null)
                    result = EditorGUILayout.TextField((string)value, style);
                else
                    result = EditorGUILayout.TextField((string)value);
            }
            else if (multiline != null || area != null || text.Type == TextFieldType.Area)
            {
                if (style != null)
                    result = EditorGUILayout.TextArea((string)value, style);
                else
                    result = EditorGUILayout.TextArea((string)value);
            }
            else if (text.Type == TextFieldType.Password)
            {
                if (style != null)
                    result = EditorGUILayout.PasswordField((string)value, style);
                else
                    result = EditorGUILayout.PasswordField((string)value);
            }
            else if (text.Type == TextFieldType.Tag)
            {
                if (style != null)
                    result = EditorGUILayout.TagField((string)value, style);
                else
                    result = EditorGUILayout.TagField((string)value);
            }
            else if (text.Type == TextFieldType.File)
            {
                if (GUILayout.Button("...", GUILayout.Height(BUTTON_HEIGHT), GUILayout.Width(BUTTON_HEIGHT * 2)))
                    result = EditorUtility.OpenFilePanel(text.Title, text.Path, text.Extension);

                if (field.Mixed)
                    GUILayout.Label("---");
                else
                    GUILayout.Label((string)value);
            }
            else if (text.Type == TextFieldType.Folder)
            {
                if (GUILayout.Button("...", GUILayout.Height(BUTTON_HEIGHT), GUILayout.Width(BUTTON_HEIGHT * 2)))
                    result = EditorUtility.OpenFolderPanel(text.Title, "", "");

                if (field.Mixed)
                    GUILayout.Label("---");
                else
                    GUILayout.Label((string)value);
            }
			
			//Custom
			else if (text.Type == TextFieldType.Prefab)
			{
				if (String.IsNullOrEmpty(text.Path))
				{
					string prefabRemoved = field.GetValue<string>().Replace("Prefabs/", "");
					int sel = Math.Max(Array.IndexOf(UnityDrawerStatics.PrefabList, prefabRemoved), 0);
					int idx = EditorGUILayout.Popup(sel, UnityDrawerStatics.PrefabList);
					result = "Prefabs/" + UnityDrawerStatics.PrefabList[idx];
				}
				else
				{
					//Debug.Log($"Pruning by {text.Path}");
					string[] pruned = PrunePrefabListByPath(UnityDrawerStatics.PrefabList, text.Path);
					string prefabRemoved = field.GetValue<string>().Replace("Prefabs/", "");
					int sel = Math.Max(Array.IndexOf(pruned, prefabRemoved), 0);
					int idx = EditorGUILayout.Popup(sel, pruned);
					result = "Prefabs/" + pruned[idx];
					//Debug.Log($"Result of prune is {result}");
				}
			}
			else if (text.Type == TextFieldType.Entity)
			{
				int sel = Math.Max(Array.IndexOf(UnityDrawerStatics.EntityList, field.GetValue<string>()), 0);

				int idx = EditorGUILayout.Popup(sel, UnityDrawerStatics.EntityList);
				result = UnityDrawerStatics.EntityList[idx];
			}
			else if (text.Type == TextFieldType.Component)
			{
				int sel = Math.Max(Array.IndexOf(UnityDrawerStatics.ComponentList, field.GetValue<string>()), 0);

				int idx = EditorGUILayout.Popup(sel, UnityDrawerStatics.ComponentList);
				result = UnityDrawerStatics.ComponentList[idx];
			}

            if (EditorGUI.EndChangeCheck())
                field.SetValue(result);

            EditorGUIUtility.labelWidth = width;

            GUILayout.EndHorizontal();
        }

	    public string[] PrunePrefabListByPath(string[] prefabList, string inPath)
	    {
		    List<String> newList = new List<string>();
		    foreach (var prefab in prefabList)
		    {
			    if (prefab.Contains(inPath))
			    {
				    newList.Add(prefab);
			    }
		    }

		    return newList.ToArray();
	    }
    }
}