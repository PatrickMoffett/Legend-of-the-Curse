using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelGenerator), editorForChildClasses: true)]
public class GameEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        LevelGenerator e = target as LevelGenerator;
        if (GUILayout.Button("DrawPrefab"))
            e.TestDraw();
        if (GUILayout.Button("DrawPath"))
            e.TestDrawPath();
    }
}