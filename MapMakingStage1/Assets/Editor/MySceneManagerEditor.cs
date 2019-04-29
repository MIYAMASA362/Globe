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

    //SceneAsset
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

    //Editor GUI
    GUIStyle guiStyle = new GUIStyle();

    bool initialize = false;
    public void Init()
    {
        folding_Galaxy = false;
        folding_Galaxys = new bool[Galaxies.Count];
        guiStyle.fontSize = 20;

        RegisterEditor();
        initialize = true;
    }

    public void Change_FoldingGalaxys_Length()
    {
        bool[] old = folding_Galaxys;   //古い
        folding_Galaxys = new bool[Galaxies.Count]; //新しい

        //ループ回数
        int Count = folding_Galaxys.Length >= old.Length ? old.Length : folding_Galaxys.Length;

        for(int i = 0; i < Count; i++)
            folding_Galaxys[i] = old[i];
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

            EditorGUILayout.BeginHorizontal();  //縦軸レイアウト

            GUILayout.FlexibleSpace();          //終端までスペース

            //銀河を追加
            if (GUILayout.Button("Add Galaxy", GUILayout.Width(100)))
            {
                Galaxies.Add(new Galaxy());
                Change_FoldingGalaxys_Length();
            }

            GUILayout.Space(10);

            //銀河の削除
            if (GUILayout.Button("Delete Galaxy", GUILayout.Width(100)))
            {
                if (Galaxies.Count != 0)
                {
                    Galaxies.RemoveAt(Galaxies.Count - 1);
                    Change_FoldingGalaxys_Length();
                }
            }

            GUILayout.Space(20);
            EditorGUILayout.EndHorizontal();    //縦軸レイアウト終了


            //銀河の内容を表示
            for (int i = 0; i < Galaxies.Count; i++)
            {
                EditorGUI.indentLevel++;        //インデント

                //各銀河の内容
                if (folding_Galaxys[i] = EditorGUILayout.Foldout(folding_Galaxys[i], "Galaxy:"+i))
                {
                    //惑星の選択画面
                    Galaxies[i].PlanetSelect = (SceneAsset)EditorGUILayout.ObjectField("Planet Select", Galaxies[i].PlanetSelect, typeof(SceneAsset), false);
                    EditorGUI.indentLevel++;    //インデント

                    EditorGUILayout.BeginHorizontal();  //縦軸レイアウト
                    GUILayout.FlexibleSpace();  //終端までスペース

                    //惑星を追加
                    if (GUILayout.Button("Add Planet", GUILayout.Width(100)))
                        Galaxies[i].Planets.Add(null);

                    GUILayout.Space(10);

                    //惑星を削除
                    if (GUILayout.Button("delete Planet", GUILayout.Width(100)))
                    {
                        if (Galaxies[i].Planets.Count != 0)
                            Galaxies[i].Planets.RemoveAt(Galaxies[i].Planets.Count - 1);
                    }

                    GUILayout.Space(20);
                    EditorGUILayout.EndHorizontal();    //縦軸レイアウト終了

                    //惑星の表示
                    for (int j = 0; j < Galaxies[i].Planets.Count; j++)
                        Galaxies[i].Planets[j] = (SceneAsset)EditorGUILayout.ObjectField("Planet:" + j, Galaxies[i].Planets[j], typeof(SceneAsset), false);

                    EditorGUI.indentLevel--;    //インデント
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

        Galaxies = new List<Galaxy>();

        foreach(var galaxy in mySceneManager.Galaxies)
        {
            Galaxy AddGalaxy = new Galaxy();
            AddGalaxy.PlanetSelect = AssetDatabase.LoadAssetAtPath<SceneAsset>(galaxy.Path_PlanetSelect);

            foreach(var planet in galaxy.Path_Planets)
            {
                AddGalaxy.Planets.Add(AssetDatabase.LoadAssetAtPath<SceneAsset>(planet));
            }

            Galaxies.Add(AddGalaxy);
        }

        Change_FoldingGalaxys_Length();
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

        foreach (var galaxy in Galaxies)
        {
            MySceneManager.Galaxy add = new MySceneManager.Galaxy();

            string Path_PlanetSelect = AssetDatabase.GetAssetPath(galaxy.PlanetSelect);

            if (string.IsNullOrEmpty(Path_PlanetSelect)) continue;
            add.Path_PlanetSelect = Path_PlanetSelect;

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
            string galaxyPath = AssetDatabase.GetAssetPath(galaxy.PlanetSelect);

            if (string.IsNullOrEmpty(galaxyPath)) continue;
            editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(galaxyPath, true));

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
