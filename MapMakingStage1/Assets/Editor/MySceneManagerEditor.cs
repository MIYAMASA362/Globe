using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(MySceneManager))]
public class MySceneManagerEditor : Editor
{
    [System.Serializable]
    public class Galaxy
    {
        public SceneAsset PlanetSelect = null;
        public List<SceneAsset> Planets = new List<SceneAsset>();
    }

    private SceneAsset Manager;
    private SceneAsset Pause;
    private SceneAsset Opening;
    private SceneAsset Title;
    private SceneAsset Option;
    private SceneAsset DataCheck;
    private SceneAsset GameStart;
    private SceneAsset GalaxySelect;
    private List<Galaxy> Galaxies = new List<Galaxy>();

    bool folding_Galaxy = false;
    bool[] folding_Galaxys;
    bool[][] folding_Planets;

    bool initialize = false;
    public void Init()
    {
        folding_Galaxy = false;
        folding_Galaxys = new bool[Galaxies.Count];
        initialize = true;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        if (!initialize) Init();
        
        Manager = (SceneAsset)EditorGUILayout.ObjectField("Manager Scene", Manager, typeof(SceneAsset), false);
        Pause = (SceneAsset)EditorGUILayout.ObjectField("Pause Scene", Pause, typeof(SceneAsset), false);

        GUILayout.Space(4);
        Opening  = (SceneAsset)EditorGUILayout.ObjectField("Opening Scene", Opening, typeof(SceneAsset), false);
        Title = (SceneAsset)EditorGUILayout.ObjectField("Title Scene", Title, typeof(SceneAsset), false);
        Option = (SceneAsset)EditorGUILayout.ObjectField("Option Scene", Option, typeof(SceneAsset), false);
        
        GUILayout.Space(4);
        GameStart = (SceneAsset)EditorGUILayout.ObjectField("GameStart Scene", GameStart, typeof(SceneAsset), false);
        DataCheck = (SceneAsset)EditorGUILayout.ObjectField("DataCheck Scene", DataCheck, typeof(SceneAsset), false);

        GUILayout.Space(4);
        if (folding_Galaxy = EditorGUILayout.Foldout(folding_Galaxy, "Galaxys"))
        {
            EditorGUI.indentLevel++;

            GalaxySelect = (SceneAsset)EditorGUILayout.ObjectField("GalaxySelect Scene", GalaxySelect, typeof(SceneAsset), false);

            EditorGUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();
            //銀河を追加
            if (GUILayout.Button("Add Galaxy", GUILayout.Width(100)))
            {
                Galaxies.Add(new Galaxy());
                folding_Galaxys = new bool[Galaxies.Count];
            }

            GUILayout.Space(10);
            if (GUILayout.Button("Delete Galaxy", GUILayout.Width(100)))
            {
                if (Galaxies.Count != 0)
                {
                    Galaxies.RemoveAt(Galaxies.Count - 1);
                    folding_Galaxys = new bool[Galaxies.Count];
                }
            }

            EditorGUILayout.EndHorizontal();


            //銀河の内容を表示
            for (int i = 0; i < Galaxies.Count; i++)
            {
                EditorGUI.indentLevel++;
                //各銀河の内容
                if (folding_Galaxys[i] = EditorGUILayout.Foldout(folding_Galaxys[i], "Galaxy:"+i))
                {
                    Galaxies[i].PlanetSelect = (SceneAsset)EditorGUILayout.ObjectField("Planet Select", Galaxies[i].PlanetSelect, typeof(SceneAsset), false);
                    EditorGUI.indentLevel++;

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Add Planet", GUILayout.Width(100)))
                    {
                        Galaxies[i].Planets.Add(null);
                    }

                    GUILayout.Space(10);
                    if (GUILayout.Button("delete Planet", GUILayout.Width(100)))
                    {
                        if (Galaxies[i].Planets.Count != 0)
                        {
                            Galaxies[i].Planets.RemoveAt(Galaxies[i].Planets.Count - 1);
                        }
                    }
                    GUILayout.Space(20);
                    EditorGUILayout.EndHorizontal();

                    for (int j = 0; j < Galaxies[i].Planets.Count; j++)
                    {
                        Galaxies[i].Planets[j] = (SceneAsset)EditorGUILayout.ObjectField("Planet:" + j, Galaxies[i].Planets[j], typeof(SceneAsset), false);
                    }

                    EditorGUI.indentLevel--;
                }

                EditorGUI.indentLevel--;
            }

            GUILayout.Space(10);

            
            EditorGUI.indentLevel--;
        }

        GUILayout.Space(8);
        if (GUILayout.Button("Apply To Build Settings"))
        {
            BuildSetting();
        }
    }

    //ManagerScene.csに登録
    public void SetMySceneManager()
    {
        var mySceneManager = target as MySceneManager;
        mySceneManager.Path_Manager = AssetDatabase.GetAssetPath(Manager);
        
    }

    //EditorのBuildSettingに設定
    public void BuildSetting()
    {
        var mySceneManager = target as MySceneManager;

        List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();

        //Manager Register
        editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(mySceneManager.Asset_ManagerScene), true));

        //Title Register
        editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(mySceneManager.Asset_TitleScene), true));

        editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(mySceneManager.Asset_PauseScene), true));

        editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(mySceneManager.Asset_GalexySelect), true));

        editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(mySceneManager.Asset_OpsitionScene), true));

        editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(mySceneManager.Asset_OpeningScene), true));

        editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(mySceneManager.Asset_DataCheckScene), true));

        editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(mySceneManager.Asset_GameStartScene), true));

        foreach (var galaxy in Galaxies)
        {
            string galaxyPath = AssetDatabase.GetAssetPath(galaxy.PlanetSelect);
            if (!string.IsNullOrEmpty(galaxyPath))
            {
                editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(galaxyPath, true));

                foreach (var planet in galaxy.Planets)
                {
                    string planetPath = AssetDatabase.GetAssetPath(planet);
                    if (!string.IsNullOrEmpty(planetPath))
                        editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(planetPath, true));
                }
            }
        }

        EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();

        Debug.Log("Success!");
    }
}
