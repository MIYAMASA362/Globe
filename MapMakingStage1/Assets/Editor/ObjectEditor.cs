using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectEditor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

//[CustomEditor(typeof(Crystal))]
//public class CrystalEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        EditorGUILayout.HelpBox(
//            "Rot Speedはクリスタルの回転スピード。" +"\n"+
//            "PlanetSceneには各Sceneの[ EventSystem ]もしくは[ PlanetSystem ]の中のPlanetScene.csを入れます。" + "\n"+
//            "PlanetSceneにこのCrystalを登録しないと判定しないので注意!",
//            MessageType.None);

//        base.OnInspectorGUI();
//    }
//}
