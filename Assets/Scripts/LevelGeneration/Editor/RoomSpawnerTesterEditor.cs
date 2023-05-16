using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoomSpawnerTester), editorForChildClasses: true)]
public class RoomSpawnerTesterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        RoomSpawnerTester e = target as RoomSpawnerTester;
        if (GUILayout.Button("DrawPrefab"))
            e.TestDraw();
    }
}