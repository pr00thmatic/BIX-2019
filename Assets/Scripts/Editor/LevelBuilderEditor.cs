using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(LevelBuilder))]
public class LevelBuilderEditor : Editor {
    LevelBuilder Target { get => (LevelBuilder) target; }

    public override void OnInspectorGUI () {
        DrawDefaultInspector();
        if (GUILayout.Button("Build")) {
            Target.Build();
        }
    }

    public static void DrawGizmos (LevelBuilder customTarget) {
        Handles.matrix = customTarget.transform.localToWorldMatrix;
    }

    void OnSceneGUI () {
        DrawGizmos(Target);
    }
}
