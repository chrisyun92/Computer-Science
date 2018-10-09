using UnityEngine;
//using UnityEditor;

public static class Util {
//#if UNITY_EDITOR
    public static void DrawString(string text, Vector3 worldPos, Color? colour = null) {
        //Handles.BeginGUI();
        if (colour.HasValue) GUI.color = colour.Value;
        //var view = UnityEditor.SceneView.currentDrawingSceneView;
        //Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
        Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
        //GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y), text);
        //Handles.EndGUI();
    }
//#endif
//#if UNITY_EDITOR
    public static void DrawLines(Vector3 start, Vector3 end, float width) {
        Vector3 v1 = (end - start).normalized; // line direction
        Vector3 n = new Vector3(-v1.y, v1.x); // normal
        Vector3 o = n * width / 2;
        Gizmos.DrawLine(start + o, end + o);
        Gizmos.DrawLine(start + (o * -1), end + (o * -1));
    }
//#endif
} 
