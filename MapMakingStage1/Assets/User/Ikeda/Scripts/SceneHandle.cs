using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneHandle : MonoBehaviour
{

    //--- Attribute ---------------------------------------

    //--- private -------------------------------
    [Header("State")]
    [SerializeField,Tooltip("このSceneがPause画面を使うか")]
    private bool IsPausing = true;

    [Space(2),Header("Scene")]

    [SerializeField,Tooltip("次のシーン")]
    private SceneIndex.ENUM_SCENE Next_Scene = SceneIndex.ENUM_SCENE.Title;

    [SerializeField,Tooltip("次のシーン群")]
    private SceneIndex.ENUM_SCENE[] Next_SceneIndex = null;

    [Space(2),Header("KeyCode")]

    [SerializeField, Tooltip("Sceneの決定キー")]
    private KeyCode TransCode = KeyCode.KeypadEnter;

    [SerializeField,Tooltip("Scene選択 Up")]
    private KeyCode SelectUp = KeyCode.D;

    [SerializeField, Tooltip("Scene選択 Down")]
    private KeyCode SelectDown = KeyCode.A;

    //Don't Serialize
    private int Selecter = 0;   //選択中のSceneIndex要素番号
    private int nLength = 0;    //Next_SceneIndex長

    //--- MonoBehavior ------------------------------------

    void Start ()
    {
        Selecter = 0;
        nLength = Next_SceneIndex.Length;
	}

    private void Update()
    {
        OnPause();
        SelectChange();
        LoadScene();
    }

    //--- Method ------------------------------------------

    //Pause画面
    void OnPause()
    {
        if (IsPausing) MySceneManager.Pause();
    }

    //Scene選択切り替え
    void SelectChange()
    {
        //Next_Sceneへの設定がある時
        if (nLength != 0)
        {
            if (Input.GetKeyDown(SelectUp))   Selecter++; //CountUp
            if (Input.GetKeyDown(SelectDown)) Selecter--; //CountDown
            if (Selecter <= -1) Selecter = nLength - 1;   //マイナス値除外

            Selecter    = Selecter % nLength;             //Selecterのループ
            Next_Scene  = Next_SceneIndex[Selecter];      //Next_SceneIndexの要素を取得
        }
    }

    //Scene遷移
    void LoadScene() 
    {
        if (Input.GetKeyDown(TransCode)) MySceneManager.LoadScene(Next_Scene, false, false);
    }

    //--- Editor ------------------------------------------
#if UNITY_EDITOR

    [CustomEditor(typeof(SceneHandle))]
    [CanEditMultipleObjects]
    private class ThisEditer : Editor
    {
        public override void OnInspectorGUI()
        {
            var sceneHandle = target as SceneHandle;

            EditorGUILayout.HelpBox(
                "SceneWindowに設定したSceneAssetsがSceneIndex.csに登録されていなければ" +
                "ここのEnumTabに表示される事はない！気を付けろ!" +
                "ちなみにManagerSceneとかにDontDestroyOnLoad関数使ってるとバグに繋がるからね",
                MessageType.None);

            base.OnInspectorGUI();
        }
    }

#endif
}