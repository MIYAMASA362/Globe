using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(PlanetScene))]
public class PlanetSceneEditor : Editor
{
    private void OnEnable()
    {
        var myPlanetScene = target as PlanetScene;

        myPlanetScene.LoadData();
    }

    public override void OnInspectorGUI()
    {
        var myPlanetScene = target as PlanetScene;

        base.OnInspectorGUI();

        GUILayout.Space(10);
        if (GUILayout.Button("Apply To Local Data")) ;// myPlanetScene.SaveData();
    }
}
