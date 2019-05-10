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
        public SceneAsset[] Planets = new SceneAsset[5];
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
    private Galaxy[] Galaxies = new Galaxy[4];
    
    bool folding_Galaxy = false;
    bool[] folding_Galaxys;

    //Editor GUI
    GUIStyle guiStyle = new GUIStyle();

    bool initialize = false;
    public void Init()
    {
        folding_Galaxy = false;
        folding_Galaxys = new bool[Galaxies.Length];
        guiStyle.fontSize = 20;

        RegisterEditor();
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

        //基本シーン
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

        //Galaxysの隠ぺい表示
        if (folding_Galaxy = EditorGUILayout.Foldout(folding_Galaxy, "Galaxys"))
        {
            EditorGUI.indentLevel++;    //インデント

            //Galaxyの選択画面
            GalaxySelect = (SceneAsset)EditorGUILayout.ObjectField("GalaxySelect Scene", GalaxySelect, typeof(SceneAsset), false);

            //銀河の内容を表示
            for (int i = 0; i < Galaxies.Length; i++)
            {
                EditorGUI.indentLevel++;        //インデント

                //各銀河の内容
                if (folding_Galaxys[i] = EditorGUILayout.Foldout(folding_Galaxys[i], "Galaxy:"+(i+1)))
                {
                    EditorGUI.indentLevel++;
                    //惑星の表示
                    for (int j = 0; j < Galaxies[i].Planets.Length; j++)
                        Galaxies[i].Planets[j] = (SceneAsset)EditorGUILayout.ObjectField("Planet:" + (j+1), Galaxies[i].Planets[j], typeof(SceneAsset), false);

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
            RegisterMySceneManager();
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

        //MySceneManagerから配列内容を確保

        int nGalaxy = 0;
        foreach (var galaxy in mySceneManager.Galaxies)
        {
            Galaxy AddGalaxy = new Galaxy();

            int nPlanet = 0;
            foreach (var planet in galaxy.Path_Planets)
            {
                if (planet == null) continue;
                AddGalaxy.Planets[nPlanet] = AssetDatabase.LoadAssetAtPath<SceneAsset>(planet);
                nPlanet++;
            }
            Galaxies[nGalaxy] = AddGalaxy;
            nGalaxy++;
        }
    }

    //ManagerScene.csに登録
    public void RegisterMySceneManager()
    {
        var mySceneManager = target as MySceneManager;

        //Manager
        mySceneManager.Path_Manager = AssetDatabase.GetAssetPath(Manager);

        //Title
        mySceneManager.Path_Title = AssetDatabase.GetAssetPath(Title);

        //Pause
        mySceneManager.Path_Pause = AssetDatabase.GetAssetPath(Pause);

        //Opening
        mySceneManager.Path_Opening = AssetDatabase.GetAssetPath(Opening);

        //GalaxySelect
        mySceneManager.Path_GalaxySelect = AssetDatabase.GetAssetPath(GalaxySelect);

        //Option
        mySceneManager.Path_Option = AssetDatabase.GetAssetPath(Option);

        //DataCheck
        mySceneManager.Path_DataCheck = AssetDatabase.GetAssetPath(DataCheck);

        //GameStart
        mySceneManager.Path_GameStart = AssetDatabase.GetAssetPath(GameStart);

        //Galaxys
        List<MySceneManager.Galaxy> AddGalaxies = new List<MySceneManager.Galaxy>();

        //プラネットの登録
        foreach (var galaxy in Galaxies)
        {
            MySceneManager.Galaxy add = new MySceneManager.Galaxy();

            add.Path_Planets = new List<string>();
            
            foreach (var planet in galaxy.Planets)
            {
                string Path_Planet = AssetDatabase.GetAssetPath(planet);

                if (string.IsNullOrEmpty(Path_Planet)) continue;
                add.Path_Planets.Add(Path_Planet);
            }

            AddGalaxies.Add(add);
        }

        mySceneManager.Galaxies = AddGalaxies;
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
