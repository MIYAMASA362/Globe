using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

[CanEditMultipleObjects]
[CustomEditor(typeof(PlanetState))]
public class PlanetStateEditor : Editor
{
    private SceneAsset sceneAsset;

    private void OnEnable()
    {
        
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var myPlanetState = target as PlanetState;

        GUILayout.Space(10);

        sceneAsset = EditorGUILayout.ObjectField("Apply Scene",sceneAsset,typeof(SceneAsset),true) as SceneAsset;

        if (GUILayout.Button("Apply To DataFile"))
        {
            string FileName;
            FileName = sceneAsset.ToString();

            
            for(int i = 0; i<FileName.Length; i++)
            {
                if(FileName[i] == '(')
                {
                    myPlanetState.DataFile = FileName.Remove(i,FileName.Length -i);
                    break;
                }
            }

            myPlanetState.LoadData();

            Debug.Log("DataFile:"+myPlanetState.DataFile);
        }
    }
}

