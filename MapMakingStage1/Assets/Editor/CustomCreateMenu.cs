using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class CustomCreateMenu
{
    public GameObject Object;
    public static GameObject stage;

    [MenuItem("GameObject/Method/SetEditPlanet",priority = 20)]
    public static void Set_EditPlanet()
    {
        stage = Selection.activeGameObject;
        Debug.Log("Target Stage:"+stage);
    }

    [MenuItem("GameObject/MyObject/Crystal",priority = 19)]
    public static void Create_Crystal()
    {
        GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/StageObjects/Crystal.prefab", typeof(GameObject));
        GameObject gameObject = (GameObject)GameObject.Instantiate(prefab);
        gameObject.transform.parent = GameObject.Find(stage.name + "/PlanetHolder/Crystals").transform;
        PrefabUtility.ConnectGameObjectToPrefab(gameObject,prefab);
    }

    [MenuItem("GameObject/MyObject/Arch")]
    public static void Create_Arch()
    {
        GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/StageObjects/NormalGround/Arch.prefab", typeof(GameObject));
        GameObject gameObject = (GameObject)GameObject.Instantiate(prefab);
        PrefabUtility.ConnectGameObjectToPrefab(gameObject, prefab);
    }

    [MenuItem("GameObject/MyObject/ArchSmall")]
    public static void Create_ArchSmall()
    {
        GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/StageObjects/NormalGround/ArchSmall.prefab", typeof(GameObject));
        GameObject gameObject = (GameObject)GameObject.Instantiate(prefab);
        PrefabUtility.ConnectGameObjectToPrefab(gameObject, prefab);
    }

    [MenuItem("GameObject/MyObject/Hex")]
    public static void Create_Hex()
    {
        GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/StageObjects/NormalGround/Hex.prefab", typeof(GameObject));
        GameObject gameObject = (GameObject)GameObject.Instantiate(prefab);
        gameObject.transform.parent = GameObject.Find(stage.name + "/PlanetHolder/Hexs").transform;
        PrefabUtility.ConnectGameObjectToPrefab(gameObject, prefab);
    }

    [MenuItem("GameObject/MyObject/SandPivot")]
    public static void Create_SandPivot()
    {
        GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/StageObjects/NormalGround/SandPivot.prefab", typeof(GameObject));
        GameObject gameObject = (GameObject)GameObject.Instantiate(prefab);
        PrefabUtility.ConnectGameObjectToPrefab(gameObject, prefab);
    }
}
