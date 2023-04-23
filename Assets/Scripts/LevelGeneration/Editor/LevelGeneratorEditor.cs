using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelGenerator), editorForChildClasses: true)]
public class LevelGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        LevelGenerator e = target as LevelGenerator;
        if (GUILayout.Button("SpawnLevel"))
            e.SpawnLevel();
    }
}
