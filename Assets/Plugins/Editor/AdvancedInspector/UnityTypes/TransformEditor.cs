/*
using System;

using UnityEditor;
using UnityEngine;

namespace AdvancedInspector
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Transform), true)]
    public class TransformEditor : InspectorEditor
    {
        protected override void RefreshFields()
        {
            Type type = typeof(Transform);

            Fields.Add(new InspectorField(type, Instances, type.GetProperty("localPosition"),
                new DescriptorAttribute("Position", "Position of the transform relative to the parent transform.", "http://docs.unity3d.com/ScriptReference/Transform-localPosition.html")));
            Fields.Add(new InspectorField(type, Instances, type.GetProperty("localEulerAngles"),
                new DescriptorAttribute("Rotation", "The rotation of the transform relative to the parent transform's rotation.", "http://docs.unity3d.com/ScriptReference/Transform-localRotation.html")));
            Fields.Add(new InspectorField(type, Instances, type.GetProperty("localScale"),
                new DescriptorAttribute("Scale", "The scale of the transform relative to the parent.", "http://docs.unity3d.com/ScriptReference/Transform-localScale.html")));

            Fields.Add(new InspectorField(type, Instances, type.GetProperty("position"), new InspectAttribute(InspectorLevel.Advanced),
                new DescriptorAttribute("World Position", "The position of the transform in world space.", "http://docs.unity3d.com/ScriptReference/Transform-position.html")));
            Fields.Add(new InspectorField(type, Instances, type.GetProperty("rotation"), new InspectAttribute(InspectorLevel.Advanced),
                new DescriptorAttribute("World Rotation", "The rotation of the transform in world space stored as a Quaternion.", "http://docs.unity3d.com/ScriptReference/Transform-rotation.html")));
        }
    }
}
*/

using UnityEngine;
using UnityEditor;

namespace Unitilities
{

	[CustomEditor(typeof(Transform))]
	public class TransformInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			Transform t = (Transform)target;

			// Replicate the standard transform inspector gui
			//EditorGUIUtility.LookLikeControls();
			EditorGUIUtility.labelWidth = 0;
			EditorGUIUtility.fieldWidth = 0;

			EditorGUI.indentLevel = 0;

			float buttonWidth = 20, labelWidth = 75, fieldWidth = 150;
			Vector3 position = t.localPosition;

			GUILayout.BeginHorizontal();
			Vector3 worldPosition = t.position;
			EditorGUILayout.LabelField("World Pos:", GUILayout.Width(labelWidth));
			if (GUILayout.Button("Z", GUILayout.Width(buttonWidth)))
			{
				position += new Vector3(-worldPosition.x, -worldPosition.y, -worldPosition.z);
			}
			GUILayout.Space(8);
			worldPosition = EditorGUILayout.Vector3Field(GUIContent.none, worldPosition, GUILayout.MinWidth(fieldWidth));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Position", GUILayout.Width(labelWidth));
			if (GUILayout.Button("Z", GUILayout.Width(buttonWidth)))
			{
				position = Vector3.zero;
			}
			GUILayout.Space(8);
			position = EditorGUILayout.Vector3Field(GUIContent.none, position, GUILayout.MinWidth(fieldWidth));
			GUILayout.EndHorizontal();


			GUILayout.BeginHorizontal();
			Vector3 eulerAngles = t.localEulerAngles;
			EditorGUILayout.LabelField("Rotation", GUILayout.Width(labelWidth));
			if (GUILayout.Button("Z", GUILayout.Width(buttonWidth)))
			{
				eulerAngles = Vector3.zero;
			}
			GUILayout.Space(8);
			eulerAngles = EditorGUILayout.Vector3Field(GUIContent.none, eulerAngles, GUILayout.MinWidth(fieldWidth));
			GUILayout.EndHorizontal();


			GUILayout.BeginHorizontal();
			Vector3 scale = t.localScale;
			EditorGUILayout.LabelField("Scale", GUILayout.Width(labelWidth));
			if (GUILayout.Button("O", GUILayout.Width(buttonWidth)))
			{
				scale = Vector3.one;
			}
			GUILayout.Space(8);
			scale = EditorGUILayout.Vector3Field(GUIContent.none, scale, GUILayout.MinWidth(fieldWidth));
			GUILayout.EndHorizontal();

			//position = EditorGUILayout.Vector3Field("Position", t.localPosition);
			//eulerAngles = EditorGUILayout.Vector3Field("Rotation", t.localEulerAngles);
			//scale = EditorGUILayout.Vector3Field("Scale", t.localScale);

			//EditorGUIUtility.LookLikeInspector();
			EditorGUIUtility.labelWidth = 0;
			EditorGUIUtility.fieldWidth = 0;


			//EditorGUIUtility.LookLikeControls

			if (GUI.changed)
			{
				Undo.RecordObject(t, "Transform Change");


				t.localPosition = FixIfNaN(position);

				t.localEulerAngles = FixIfNaN(eulerAngles);
				t.localScale = FixIfNaN(scale);
			}
		}

		private Vector3 FixIfNaN(Vector3 v)
		{
			if (float.IsNaN(v.x))
			{
				v.x = 0;
			}
			if (float.IsNaN(v.y))
			{
				v.y = 0;
			}
			if (float.IsNaN(v.z))
			{
				v.z = 0;
			}
			return v;
		}

	}

}


/*
using System;

using UnityEditor;
using UnityEngine;

namespace AdvancedInspector
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(Transform), true)]
	public class TransformEditor : InspectorEditor
	{
		protected override void RefreshFields()
		{
			Type type = typeof(Transform);

			//Fields.Add(new InspectorField());

			Fields.Add(new InspectorField(type, Instances, type.GetProperty("position"), // new InspectAttribute(InspectorLevel.Advanced),
			                              new DescriptorAttribute("WP", "The position of the transform in world space.", "http://docs.unity3d.com/ScriptReference/Transform-position.html")));
			Fields.Add(new InspectorField(type, Instances, type.GetProperty("localPosition"),
			                              new DescriptorAttribute("P", "Position of the transform relative to the parent transform.", "http://docs.unity3d.com/ScriptReference/Transform-localPosition.html")));
			Fields.Add(new InspectorField(type, Instances, type.GetProperty("localEulerAngles"),
			                              new DescriptorAttribute("R", "The rotation of the transform relative to the parent transform's rotation.", "http://docs.unity3d.com/ScriptReference/Transform-localRotation.html")));
			Fields.Add(new InspectorField(type, Instances, type.GetProperty("localScale"),
			                              new DescriptorAttribute("S", "The scale of the transform relative to the parent.", "http://docs.unity3d.com/ScriptReference/Transform-localScale.html")));

			//Fields.Add(new InspectorField(type, Instances, type.GetProperty("rotation"), new InspectAttribute(InspectorLevel.Advanced),
			//    new DescriptorAttribute("World Rotation", "The rotation of the transform in world space stored as a Quaternion.", "http://docs.unity3d.com/ScriptReference/Transform-rotation.html")));
		}

	}
}

*/
