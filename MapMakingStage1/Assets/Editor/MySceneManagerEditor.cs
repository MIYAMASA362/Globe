using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(MySceneManager))]
public class MySceneManagerEditor : Editor
{
    [System.Serializable]
    public class Planet
    {
        public string Name = "NONE";
        public SceneAsset sceneAsset = null;
    }

    [System.Serializable]
    public class Galaxy
    {
        public string Name = "NONE";
        public List<Planet> Planets = new List<Planet>();
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
    private SceneAsset End;
    
    bool folding_Galaxy = false;
    bool[] folding_Galaxys = new bool[4];

    //Editor GUI
    GUIStyle guiStyle = new GUIStyle();

    bool initialize = false;

    public void Init()
    {
        guiStyle.fontSize = 20;

        Load_State();
        folding_Galaxy = false;
        folding_Galaxys = new bool[Galaxies.Count];

        initialize = true;
    }

    public override void OnInspectorGUI()
    {

        //ターゲットを設定
        MySceneManager mySceneManager = MySceneManager.Instance;

        //MySceneManagerの内容を表示するか
        if (true)
        {
            base.OnInspectorGUI();
            GUILayout.Space(8);
        }

        
        //初期化
        if (!initialize) Init();
        Debug.Log(folding_Galaxys.Length);

        //Haeder
        EditorGUILayout.LabelField("MySceneManager Editor", guiStyle);
        GUILayout.Space(15);

        //State

        GUILayout.Space(4);
        Manager = EditorGUILayout.ObjectField("Manager",Manager,typeof(SceneAsset),true) as SceneAsset;
        Pause = EditorGUILayout.ObjectField("Pause",Pause,typeof(SceneAsset),true) as SceneAsset;

        GUILayout.Space(4);
        Opening = EditorGUILayout.ObjectField("Opening", Opening, typeof(SceneAsset), true) as SceneAsset;
        Title = EditorGUILayout.ObjectField("Title", Title, typeof(SceneAsset), true) as SceneAsset;
        Option = EditorGUILayout.ObjectField("Option", Option, typeof(SceneAsset), true) as SceneAsset;

        GUILayout.Space(4);
        DataCheck = EditorGUILayout.ObjectField("DataCheck", DataCheck, typeof(SceneAsset), true) as SceneAsset;
        GameStart = EditorGUILayout.ObjectField("GameStart", GameStart, typeof(SceneAsset), true) as SceneAsset;

        GUILayout.Space(4);

        if (folding_Galaxy = EditorGUILayout.Foldout(folding_Galaxy, "Galaxys"))
        {
            EditorGUI.indentLevel++;    //インデント

            //Galaxyの選択画面
            GalaxySelect = EditorGUILayout.ObjectField("StageSelect", GalaxySelect, typeof(SceneAsset), true) as SceneAsset;

            EditorGUI.indentLevel++;    //インデント
            for (int nGalaxy = 0; nGalaxy < Galaxies.Count; nGalaxy++)
            {
                if (folding_Galaxys[nGalaxy] = EditorGUILayout.Foldout(folding_Galaxys[nGalaxy], "Galaxy:" + (nGalaxy + 1)))
                {
                    EditorGUI.indentLevel++;    //インデント
                    Galaxies[nGalaxy].Name = EditorGUILayout.TextField("Name",Galaxies[nGalaxy].Name);

                    for (int nPlanet = 0; nPlanet < Galaxies[nGalaxy].Planets.Count; nPlanet++)
                    {
                        EditorGUILayout.LabelField("Planet:"+(nPlanet+1));
                        EditorGUI.indentLevel++;
                        Galaxies[nGalaxy].Planets[nPlanet].Name = EditorGUILayout.TextField("Name", Galaxies[nGalaxy].Planets[nPlanet].Name);
                        Galaxies[nGalaxy].Planets[nPlanet].sceneAsset = EditorGUILayout.ObjectField("SceneAsset", Galaxies[nGalaxy].Planets[nPlanet].sceneAsset, typeof(SceneAsset), true) as SceneAsset;
                        EditorGUI.indentLevel--;
                    }
                    EditorGUI.indentLevel--;    //インデント
                }
            }
            EditorGUI.indentLevel--;    //インデント

            GUILayout.Space(10);

            EditorGUI.indentLevel--;
        }

        GUILayout.Space(4);
        End = EditorGUILayout.ObjectField("EndGame",End,typeof(SceneAsset),true)as SceneAsset;

        GUILayout.Space(8);

        //BuildSettingへ登録
        if (GUILayout.Button("Apply To Build Settings"))
        {
            BuildSetting();
            Save_State();
            AssetDatabase.SaveAssets();
            Create_DataFile();
        }

        if(GUILayout.Button("Create DataFile"))
        {
            Create_DataFile();
        }
    }

    //MySceneManagerからデータを取得
    public void Load_State()
    {
        //ターゲットを設定
        MySceneManager mySceneManager = MySceneManager.Instance;

        //Scene情報の取得

        Manager = AssetDatabase.LoadAssetAtPath<SceneAsset>(mySceneManager.Path_Manager);
        Pause = AssetDatabase.LoadAssetAtPath<SceneAsset>(mySceneManager.Path_Pause);

        Opening = AssetDatabase.LoadAssetAtPath<SceneAsset>(mySceneManager.Path_Opening);
        Title = AssetDatabase.LoadAssetAtPath<SceneAsset>(mySceneManager.Path_Title);
        Option = AssetDatabase.LoadAssetAtPath<SceneAsset>(mySceneManager.Path_Option);

        DataCheck = AssetDatabase.LoadAssetAtPath<SceneAsset>(mySceneManager.Path_DataCheck);
        GameStart = AssetDatabase.LoadAssetAtPath<SceneAsset>(mySceneManager.Path_GameStart);

        GalaxySelect = AssetDatabase.LoadAssetAtPath<SceneAsset>(mySceneManager.Path_GalaxySelect);

        foreach(var Galaxy in mySceneManager.Galaxies)
        {
            Galaxy galaxy = new Galaxy();
            galaxy.Name = Galaxy.name;
            foreach(var Planet in Galaxy.Planets)
            {
                MySceneManagerEditor.Planet add = new MySceneManagerEditor.Planet();
                add.Name = (Planet.name == ""? add.Name:Planet.name);
                add.sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(Planet.Path);
                galaxy.Planets.Add(add);
            }
            Galaxies.Add(galaxy);
        }

        End = AssetDatabase.LoadAssetAtPath<SceneAsset>(mySceneManager.Path_End);
    }

    //MySceneManagerへデータを保存
    public void Save_State()
    {
        //ターゲットを設定
        MySceneManager mySceneManager = MySceneManager.Instance;

        mySceneManager.Path_Manager = AssetDatabase.GetAssetPath(Manager);
        mySceneManager.Path_Pause = AssetDatabase.GetAssetPath(Pause);
        mySceneManager.Path_Opening = AssetDatabase.GetAssetPath(Opening);
        mySceneManager.Path_Title = AssetDatabase.GetAssetPath(Title);
        mySceneManager.Path_Option = AssetDatabase.GetAssetPath(Option);
        mySceneManager.Path_DataCheck = AssetDatabase.GetAssetPath(DataCheck);
        mySceneManager.Path_GameStart = AssetDatabase.GetAssetPath(GameStart);
        mySceneManager.Path_GalaxySelect = AssetDatabase.GetAssetPath(GalaxySelect);

        mySceneManager.Galaxies = new List<MySceneManager.Galaxy>();
        //銀河参照
        for(int nGalaxy = 0; nGalaxy < Galaxies.Count; nGalaxy++)
        {
            MySceneManager.Galaxy galaxy = new MySceneManager.Galaxy();
            galaxy.name = Galaxies[nGalaxy].Name;
            //銀河の惑星参照
            for(int nPlanet = 0; nPlanet < Galaxies[nGalaxy].Planets.Count; nPlanet++)
            {
                MySceneManager.Planet add = new MySceneManager.Planet();
                string path = AssetDatabase.GetAssetPath(Galaxies[nGalaxy].Planets[nPlanet].sceneAsset);
                Debug.Log(path);
                add.Path = path;
                add.name = Galaxies[nGalaxy].Planets[nPlanet].Name;
                galaxy.Planets.Add(add);
            }
            mySceneManager.Galaxies.Add(galaxy);
        }

        mySceneManager.Path_End = AssetDatabase.GetAssetPath(End);
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
                string planetPath = AssetDatabase.GetAssetPath(planet.sceneAsset);

                if (string.IsNullOrEmpty(planetPath)) continue;
                editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(planetPath, true));
            }
        }

        editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(End),true));

        EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();

        Debug.Log("Success!");
    }

    public void Create_DataFile()
    {
        DataHandle.Delete_LocalDirectoryData(true);

        DataType.PlayerData playerData = new DataType.PlayerData();
        DataHandle.Save(ref playerData,playerData.FileName());

        foreach(Galaxy galaxy in Galaxies)
        {
            foreach(Planet planet in galaxy.Planets)
            {
                string AssetName = planet.sceneAsset.ToString();
                for(int i = 0; i<AssetName.Length; i++)
                {
                    if (AssetName[i] != '(') continue;

                    string FileName = AssetName.Remove(i-1,(AssetName.Length+1) -i);
                    DataType.PlanetData planetData = new DataType.PlanetData(FileName);
                    DataHandle.Save(ref planetData,planetData.FileName());
                    break;
                }
            }
        }
    }
}
