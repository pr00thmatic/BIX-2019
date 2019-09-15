using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class CoolEditor {
    public static Tool oldTool;

    public static void HideTool () {
        if (Tools.current != Tool.None) {
            oldTool = Tools.current;
            Tools.current = Tool.None;
        }
    }

    public static void ShowTool () {
        if (Tools.current != Tool.None) return;
        Tools.current = oldTool == Tool.None? Tool.Move: oldTool;
    }

    public static void ArrowHead (Vector3 position, Vector3 direction,
                                  float size, float offset = 0) {
        direction.Normalize();

        Matrix4x4 newMatrix = Handles.matrix *
            Matrix4x4.TRS(position, Quaternion.LookRotation(direction),
                          Vector3.one);

        using (new Handles.DrawingScope(Handles.color, newMatrix)) {
            Handles.DrawAAConvexPolygon(new Vector3[] {
                    new Vector3(0,0, -offset),
                    new Vector3(size/2f, 0, -size -offset),
                    new Vector3(-size/2f, 0, -size -offset)
                });
        }
    }

    public static bool SpecialKeyDown () {
        return Event.current.shift ||
            Event.current.control || Event.current.alt;
    }

    public static bool ClickDown (int button = 0) {
        return Event.current.type == EventType.MouseDown &&
            Event.current.button == button && !SpecialKeyDown();
    }

    public static bool KeyDown (KeyCode key) {
        return Event.current.type == EventType.KeyDown &&
            Event.current.keyCode == key;
    }

    public static void CancelEvent () {
        GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);
        Event.current.Use();
        GUIUtility.hotControl = 0;
    }

    public static bool MouseRaycastToPlane (out Vector3 hitPoint,
                                            Vector3 planeNormal,
                                            Vector3 origin = default(Vector3)) {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Plane plane = new Plane(planeNormal, origin);
        float distance = 0;
        bool didHit = plane.Raycast(ray, out distance);
        hitPoint = ray.GetPoint(distance);
        return didHit;
    }

    public static bool DrawSolidButton (Vector3 position, float size) {
        bool didClick =
            Handles.Button(position, Quaternion.LookRotation(Vector3.up),
                           size, size, Handles.CircleHandleCap);
        Handles.DrawSolidDisc(position, Vector3.up, size);

        return didClick;
    }
}
