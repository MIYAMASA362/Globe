using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(PlanetScene))]
public class PlanetSceneEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(4);
        if (GUILayout.Button("Apply To Local Data")) Apply();
    }

    public void Apply()
    {
        var myPlanetScene = target as PlanetScene;

        myPlanetScene.ResetData();
    }
}
