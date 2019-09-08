using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Graph))]
public class GraphEditor : Editor {
    Graph Target { get => (Graph) target; }
    static float _buttonSize = 0.5f;
    public static int selected = 0;

    public static bool directedEdges = false;
    public static bool continuousConnection = false;
    public static bool createNewVertices = true;

    void OnEnable () {
        selected = -1;
        createNewVertices = true;
    }

    public static void DrawInspector () {
        float newValue = EditorGUILayout.Slider("GUI Size", _buttonSize, 0, 10);
        if (newValue != _buttonSize) {
            _buttonSize = newValue;
            SceneView.RepaintAll();
        }

        GUILayout.Space(20);

        createNewVertices =
            GUILayout.Toggle(createNewVertices, "Create new vertices",
                             "Button");

        directedEdges =
            GUILayout.Toggle(directedEdges, "Directed Edges", "Button");
        continuousConnection =
            GUILayout.Toggle(continuousConnection, "Connect new with last",
                             "Button");
    }

    public override void OnInspectorGUI () {
        DrawDefaultInspector();
        DrawInspector();
    }

    public static void DrawEdges (Graph target) {
        Matrix4x4 oldMatrix = Handles.matrix;
        Handles.matrix = target.PoV.localToWorldMatrix;

        for (int i=0; i<target.edges.Count; i++) {
            foreach (int link in target.edges[i].links) {
                Handles.DrawLine(target.vertex[link], target.vertex[i]);
                CoolEditor.ArrowHead(target.vertex[link],
                                     target.vertex[link] - target.vertex[i],
                                     _buttonSize, _buttonSize);
            }
        }

        Handles.matrix = oldMatrix;
    }

    public static int DrawVertices (Graph target) {
        Matrix4x4 oldMatrix = Handles.matrix;
        Handles.matrix = target.PoV.localToWorldMatrix;
        Color oldColor = Handles.color;
        Color normalColor = new Color(1,1,1, 0.25f);
        Color selectedColor = new Color(0.2f, 1 ,0.2f, 0.25f);

        int clicked = -1;

        for (int i=0; i<target.vertex.Count; i++) {
            Handles.color = selected == i? selectedColor: normalColor;

            if (CoolEditor.DrawSolidButton(target.vertex[i], _buttonSize)) {
                clicked = i;
            }
        }

        if (CoolEditor.KeyDown(KeyCode.Escape)) {
            selected = -1;
            CoolEditor.CancelEvent();
        }

        if (selected == clicked) {
            selected = -1;
            clicked = -1;
        }

        if (selected < 0) {
            selected = clicked >= 0? clicked: selected;
        }

        Handles.color = oldColor;
        Handles.matrix = oldMatrix;
        return clicked;
    }

    public static void UpdateEdgeCreation (Graph target, int clicked) {
        if (clicked >= 0 && selected >= 0 && selected != clicked) {
            if (target.IsConnected(selected, clicked)) {

                Undo.RecordObject(target, "disconnected two vertices");
                target.Disconnect(selected, clicked, directedEdges);

            } else {

                Undo.RecordObject(target, "connected two vertices");
                target.Connect(selected, clicked, directedEdges);

            }
        }
    }

    public static void UpdateVertexCreation (Graph target) {
        Matrix4x4 oldMatrix = Handles.matrix;
        Handles.matrix = target.PoV.localToWorldMatrix;

        Vector3 pos;
        if (CoolEditor.ClickDown()) {
            CoolEditor.MouseRaycastToPlane(out pos, target.PoV.up,
                                           target.PoV.position);
            Undo.RecordObject(target, "created vertex");
            target.AddVertex(target.ToLocalPoint(pos));
            CoolEditor.CancelEvent();

            if (continuousConnection && selected >= 0) {
                target.Connect(selected, target.vertex.Count - 1,
                               directedEdges);
            }

            selected = target.vertex.Count-1;
        }

        Handles.matrix = oldMatrix;
    }

    public static void UpdateVertexMotion (Graph target) {
        if (selected < 0) return;

        Matrix4x4 old = Handles.matrix;
        Handles.matrix = target.PoV.localToWorldMatrix;

        if (Event.current.type == EventType.MouseDrag &&
            Event.current.button == 0 && !CoolEditor.SpecialKeyDown()) {

            Undo.RecordObject(target, "moved a vertex");
            Vector3 pos;
            CoolEditor.MouseRaycastToPlane(out pos, target.PoV.up,
                                           target.PoV.position);
            target.vertex[selected] = target.ToLocalPoint(pos);
            CoolEditor.CancelEvent();

        }

        Handles.matrix = old;
    }

    public static void UpdateDeletion (Graph target) {
        if (selected < target.vertex.Count &&
            CoolEditor.KeyDown(KeyCode.Delete)) {

            Undo.RecordObject(target, "deleted vertex");
            target.DeleteVertex(selected);
            selected = -1;
            CoolEditor.CancelEvent();

        }
    }

    public static void DrawGizmos (Graph target) {
        int clicked = DrawVertices(target);
        DrawEdges(target);

        UpdateDeletion(target);
        UpdateEdgeCreation(target, clicked);

        if (createNewVertices) {
            UpdateVertexCreation(target);
            CoolEditor.HideTool();
        } else {
            CoolEditor.ShowTool();
        }

        UpdateVertexMotion(target);
    }

    void OnSceneGUI () {
        DrawGizmos(Target);
    }
}
