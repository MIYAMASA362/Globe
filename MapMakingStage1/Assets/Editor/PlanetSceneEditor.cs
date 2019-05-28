using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DataType;

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

        //sceneAsset = EditorGUILayout.ObjectField("Scene", sceneAsset, typeof(SceneAsset), true) as SceneAsset;

        if(GUILayout.Button("Load DataFile"))
        {
            myPlanetScene.LoadData();
        }

        /*
        if (GUILayout.Button("Apply To DataFile"))
        {
            string FileName;
            FileName = sceneAsset.ToString();
            
            for (int i = 0; i < FileName.Length; i++)
            {
                if (FileName[i] == '(')
                {
                    myPlanetScene.DataFile = FileName.Remove(i-1, (FileName.Length+1) - i);
                    break;
                }
            }

            myPlanetScene.LoadData();

            Debug.Log("DataFile:" + myPlanetScene.DataFile);
        }

        if(GUILayout.Button("Register Planet Data"))
        {
            myPlanetScene.SaveData();
            Debug.Log("DataFile:" + myPlanetScene.DataFile);
        }*/
    }
    
}
