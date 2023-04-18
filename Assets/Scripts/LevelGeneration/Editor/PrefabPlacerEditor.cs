using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PrefabPlacer), editorForChildClasses: true)]
public class GameEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        PrefabPlacer e = target as PrefabPlacer;
        if (GUILayout.Button("DrawPrefab"))
            e.TestDraw();
    }
}