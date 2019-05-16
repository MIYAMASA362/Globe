using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(PlanetScene))]
public class PlanetSceneEditor : Editor
{
    private SceneAsset sceneAsset;

    public void OnEnable()
    {

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var myPlanetScene = target as PlanetScene;

        GUILayout.Space(10);

        sceneAsset = EditorGUILayout.ObjectField("Apply Scene", sceneAsset, typeof(SceneAsset), true) as SceneAsset;

        if (GUILayout.Button("Apply To DataFile"))
        {
            string FileName;
            FileName = sceneAsset.ToString();


            for (int i = 0; i < FileName.Length; i++)
            {
                if (FileName[i] == '(')
                {
                    myPlanetScene.DataFile = FileName.Remove(i, FileName.Length - i);
                    break;
                }
            }

            myPlanetScene.LoadData();

            Debug.Log("DataFile:" + myPlanetScene.DataFile);
        }
    }
    
}
