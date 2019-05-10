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
        public List<SceneAsset> Planets = new List<SceneAsset> { null, null, null, null, null };
    }

    //SceneAsset
    private SceneAsset Manager;
    private SceneAsset Pause;
    private SceneAsset Opening;
    private SceneAsset Title;
    private SceneAsset Option;
    private SceneAsset DataCheck;
    private SceneAsset GameStart;
    private SceneAsset GalaxySelect;
    private List<Galaxy> Galaxies = new List<Galaxy> { new Galaxy(), new Galaxy(), new Galaxy(), new Galaxy() };
    
    bool folding_Galaxy = false;
    bool[] folding_Galaxys;

    //Editor GUI
    GUIStyle guiStyle = new GUIStyle();

    bool initialize = false;
    public void Init()
    {
        folding_Galaxy = false;
        folding_Galaxys = new bool[Galaxies.Count];
        guiStyle.fontSize = 20;

        initialize = true;
    }

    public override void OnInspectorGUI()
    {
        if (true) //MySceneManagerのInspecter表示
        {
            base.OnInspectorGUI();
            GUILayout.Space(8);
        }

        if (!initialize) Init();

        EditorGUILayout.LabelField("MySceneManager Editor",guiStyle);
        GUILayout.Space(15);
        //if (GUILayout.Button("Load")) RegisterEditor();

        MySceneManager mySceneManager = target as MySceneManager;

        //基本シーン
        Manager =  EditorGUILayout.ObjectField("Manager Scene", AssetDatabase.LoadAssetAtPath<SceneAsset>(mySceneManager.Path_Manager), typeof(SceneAsset), false) as SceneAsset;
        Pause   =  EditorGUILayout.ObjectField("Pause Scene", AssetDatabase.LoadAssetAtPath<SceneAsset>(mySceneManager.Path_Pause), typeof(SceneAsset), false) as SceneAsset;

        GUILayout.Space(4);
        Opening = EditorGUILayout.ObjectField("Opening Scene", AssetDatabase.LoadAssetAtPath<SceneAsset>(mySceneManager.Path_Opening), typeof(SceneAsset), false) as SceneAsset;
        Title   = EditorGUILayout.ObjectField("Title Scene", AssetDatabase.LoadAssetAtPath<SceneAsset>(mySceneManager.Path_Title), typeof(SceneAsset), false) as SceneAsset;
        Option  = EditorGUILayout.ObjectField("Option Scene", AssetDatabase.LoadAssetAtPath<SceneAsset>(mySceneManager.Path_Option), typeof(SceneAsset), false) as SceneAsset;
        
        GUILayout.Space(4);
        GameStart = EditorGUILayout.ObjectField("GameStart Scene", AssetDatabase.LoadAssetAtPath<SceneAsset>(mySceneManager.Path_GameStart), typeof(SceneAsset), false) as SceneAsset;
        DataCheck = EditorGUILayout.ObjectField("DataCheck Scene", AssetDatabase.LoadAssetAtPath<SceneAsset>(mySceneManager.Path_DataCheck), typeof(SceneAsset), false) as SceneAsset;

        GUILayout.Space(4);

        //Galaxysの隠ぺい表示
        if (folding_Galaxy = EditorGUILayout.Foldout(folding_Galaxy, "Galaxys"))
        {
            EditorGUI.indentLevel++;    //インデント

            //Galaxyの選択画面
            GalaxySelect = EditorGUILayout.ObjectField("GalaxySelect Scene", AssetDatabase.LoadAssetAtPath<SceneAsset>(mySceneManager.Path_GalaxySelect), typeof(SceneAsset), false) as SceneAsset;

            //銀河の内容を表示
            for (int i = 0; i < Galaxies.Count; i++)
            {
                EditorGUI.indentLevel++;        //インデント

                //各銀河の内容
                if (folding_Galaxys[i] = EditorGUILayout.Foldout(folding_Galaxys[i], "Galaxy:"+(i+1)))
                {
                    EditorGUI.indentLevel++;
                    //惑星の表示
                    for (int j = 0; j < Galaxies[i].Planets.Count; j++)
                    {
                        Galaxies[i].Planets[j] = EditorGUILayout.ObjectField("Planet:" + (j + 1), AssetDatabase.LoadAssetAtPath<SceneAsset>(mySceneManager.Galaxies[i].Path_Planets[j]), typeof(SceneAsset), false) as SceneAsset;
                    }
                    EditorGUI.indentLevel--;
                }

                EditorGUI.indentLevel--;    //インデント
            }

            GUILayout.Space(10);

            EditorGUI.indentLevel--;
        }

        GUILayout.Space(8);

        //BuildSettingへ登録
        if (GUILayout.Button("Apply To Build Settings"))
        {
            BuildSetting();
        }
    }

    //失ったEditorの情報を復元
    public void RegisterEditor()
    {
        var mySceneManager = target as MySceneManager;

        Manager = AssetDatabase.LoadAssetAtPath<SceneAsset>(mySceneManager.Path_Manager);
        Title = AssetDatabase.LoadAssetAtPath<SceneAsset>(mySceneManager.Path_Title);
        Pause = AssetDatabase.LoadAssetAtPath<SceneAsset>(mySceneManager.Path_Pause);
        Opening = AssetDatabase.LoadAssetAtPath<SceneAsset>(mySceneManager.Path_Opening);
        GalaxySelect = AssetDatabase.LoadAssetAtPath<SceneAsset>(mySceneManager.Path_GalaxySelect);
        Option = AssetDatabase.LoadAssetAtPath<SceneAsset>(mySceneManager.Path_Option);
        DataCheck = AssetDatabase.LoadAssetAtPath<SceneAsset>(mySceneManager.Path_DataCheck);
        GameStart = AssetDatabase.LoadAssetAtPath<SceneAsset>(mySceneManager.Path_GameStart);

        for(int nGalaxy = 0; nGalaxy < mySceneManager.Galaxies.Count; nGalaxy++)
        { 
            for(int nPlanet = 0; nPlanet < mySceneManager.Galaxies[nGalaxy].Path_Planets.Count; nPlanet++)
            {
                Galaxies[nGalaxy].Planets[nPlanet] = AssetDatabase.LoadAssetAtPath<SceneAsset>(mySceneManager.Galaxies[nGalaxy].Path_Planets[nPlanet]);
            }
        }
    }

    //EditorのBuildSettingに設定
    public void BuildSetting()
    {
        List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();

        //Manager
        editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(Manager), true));

        //Title
        editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(Title), true));

        //Pause
        editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(Pause), true));

        //GalaxySelect
        editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(GalaxySelect), true));

        //Option
        editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(Option), true));

        //Opening
        editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(Opening), true));

        //DataCheck
        editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(DataCheck), true));

        //GameStart
        editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(GameStart), true));

        //Galaxys
        foreach (var galaxy in Galaxies)
        {
            foreach (var planet in galaxy.Planets)
            {
                string planetPath = AssetDatabase.GetAssetPath(planet);

                if (string.IsNullOrEmpty(planetPath)) continue;
                editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(planetPath, true));
            }
        }

        EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();

        Debug.Log("Success!");
    }
}
