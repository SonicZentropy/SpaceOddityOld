using FullInspector.LayoutToolkit;
using UnityEditor;
using UnityEngine;

namespace FullInspector.Modules {

    

    //[CustomBehaviorEditor(typeof(Transform))]
    //public class TransformBehaviorEditor : BehaviorEditor<Transform> {
    //    private static fiLayout Layout;
	//
    //    static TransformBehaviorEditor() {
    //        float vecHeight = EditorStyles.label.CalcHeight(GUIContent.none, 0);
    //        Layout = new fiVerticalLayout {
    //            { "Position", vecHeight },
    //            2,
    //            { "Rotation", vecHeight },
    //            2,
    //            { "Scale", vecHeight }
    //        };
    //    }
	//
    //    protected override void OnEdit(Rect rect, Transform behavior, fiGraphMetadata metadata) {
    //        behavior.position = EditorGUI.Vector3Field(Layout.GetSectionRect("Position", rect), "Position", behavior.position);
    //        behavior.rotation = Quaternion.Euler(EditorGUI.Vector3Field(Layout.GetSectionRect("Rotation", rect), "Rotation", behavior.rotation.eulerAngles));
    //        behavior.localScale = EditorGUI.Vector3Field(Layout.GetSectionRect("Scale", rect), "Scale", behavior.localScale);
    //    }
	//
    //    protected override float OnGetHeight(Transform behavior, fiGraphMetadata metadata) {
    //        return Layout.Height;
    //    }
	//
    //    protected override void OnSceneGUI(Transform behavior) {
    //    }
    //}
}