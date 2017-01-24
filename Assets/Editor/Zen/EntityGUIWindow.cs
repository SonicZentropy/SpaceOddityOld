namespace Zenobit.Editor
{
	using AdvancedInspector;
	using Common.ZenECS;
	using Serialization;
	using UnityEngine;
	using UnityEditor;

	public class EntityGUIWindow : EditorWindow
	{
		private ExternalEditor editor;

		private EntityGUICreator egc;

		[MenuItem("Zenobit/Entity Editor Window", false, 0)]
		public static void Init()
		{
			EntityGUIWindow window = EditorWindow.GetWindow<EntityGUIWindow>();
			window.titleContent.text = "Zentity";
			window.wantsMouseMove = false;
			
			window.editor = ExternalEditor.CreateInstance<ExternalEditor>();

			window.editor.DraggableSeparator = true;
			window.editor.DivisionSeparator = 175;
			
		}
		
		private void OnGUI()
		{
			if (egc == null)
			{
				//egc = new EntityGuiCreator();
				egc = FindObjectOfType<EntityGUICreator>();
				editor.Instances = new object[] {egc};
			}

			if (ZenDraw(new Rect(0, 16, position.width, position.height - 16)))
			{
				Repaint();
			}
		}

		/// <summary>IControl implementation</summary>
		public bool ZenDraw(Rect region)
		{
			bool flag = false;
			GUILayout.BeginArea(region);
			if (editor.Instances != null)
			{
				EditorGUILayout.BeginVertical();
				editor.scrollPosition = EditorGUILayout.BeginScrollView(editor.scrollPosition);
				flag = AdvancedInspectorControl.Inspect((InspectorEditor)editor, editor.Fields, false, true, editor.Expandable, editor.separator);
				
				if (GUI.changed)
				{
					foreach (object instance in editor.Instances)
					{
						if (instance is UnityEngine.Object)
							EditorUtility.SetDirty(instance as UnityEngine.Object);
						IDataChanged idataChanged = instance as IDataChanged;
						idataChanged?.DataChanged();
					}
				}
				EditorGUILayout.EndScrollView();
				GUILayout.FlexibleSpace();
				editor.DrawProviderInfo();
				EditorGUILayout.EndVertical();
			}
			GUILayout.EndArea();
			return flag;
		}
	}

	public class EngineWindow : EditorWindow
	{
		private ExternalEditor editor;

		private EcsEngine egc;

		[MenuItem("Zenobit/ECS Engine Window", false, 1)]
		public static void Init()
		{
			EngineWindow window = EditorWindow.GetWindow<EngineWindow>();
			window.titleContent.text = "ECSEngine";
			window.wantsMouseMove = false;

			window.editor = ExternalEditor.CreateInstance<ExternalEditor>();

			window.editor.DraggableSeparator = true;
			window.editor.DivisionSeparator = 175;

		}

		private void OnGUI()
		{
			if (egc == null)
			{
				egc = GameObject.FindObjectOfType<EcsEngineWrapper>().engine;
				if (egc == null) return;
				editor.Instances = new object[] { egc };
			}

			if (ZenDraw(new Rect(0, 16, position.width, position.height - 16)))
			{
				Repaint();
			}
		}

		/// <summary>IControl implementation</summary>
		public bool ZenDraw(Rect region)
		{
			bool flag = false;
			GUILayout.BeginArea(region);
			if (editor.Instances != null)
			{
				EditorGUILayout.BeginVertical();
				editor.scrollPosition = EditorGUILayout.BeginScrollView(editor.scrollPosition);
				flag = AdvancedInspectorControl.Inspect((InspectorEditor)editor, editor.Fields, false, true, editor.Expandable, editor.separator);

				if (GUI.changed)
				{
					foreach (object instance in editor.Instances)
					{
						if (instance is UnityEngine.Object)
							EditorUtility.SetDirty(instance as UnityEngine.Object);
						IDataChanged idataChanged = instance as IDataChanged;
						idataChanged?.DataChanged();
					}
				}
				EditorGUILayout.EndScrollView();
				GUILayout.FlexibleSpace();
				editor.DrawProviderInfo();
				EditorGUILayout.EndVertical();
			}
			GUILayout.EndArea();
			return flag;
		}
	}
}