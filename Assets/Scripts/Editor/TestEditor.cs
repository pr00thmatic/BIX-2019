using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Test))]
public class TestEditor : Editor {
    Test Target { get => (Test) target; }
    public int personal = 0;
    public static int nailed = 0;
    int x = 0;

    public override void OnInspectorGUI () {
        if (GUILayout.Button("1")) {
            x = 1;
        }

        if (GUILayout.Button("2")) {
            x = 2;
        }

        if (GUILayout.Button("3")) {
            x = 3;
        }

        DrawDefaultInspector();
    }

    public static void DrawGizmos (Test customTarget) {
        Handles.matrix = customTarget.transform.localToWorldMatrix;
    }

    void OnSceneGUI () {
        Debug.Log(x);

        DrawGizmos(Target);
    }
}
