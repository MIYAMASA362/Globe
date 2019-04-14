using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneTransition : MonoBehaviour
{
    //--- Attribute ---------------------------------------

    //--- private -------------------------------
    [Header("State")]
    [SerializeField,Tooltip("このSceneがPause画面を使うか")]
    private bool IsPausing = true;

    [Space(4),Header("Scene")]

    [SerializeField,Tooltip("次のシーン")]
    private SceneIndex.ENUM_SCENE Next_Scene = SceneIndex.ENUM_SCENE.Title;

    [SerializeField,Tooltip("次のシーン群")]
    private SceneIndex.ENUM_SCENE[] Next_SceneIndex = null;

    [Space(4),Header("KeyCode")]

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

	void Update ()
    {
        //Pause画面
        if (IsPausing)
            MySceneManager.Pause();

        //Scene選択切り替え
        if(nLength != 0)
        {
            if (Input.GetKeyDown(SelectUp))         //CountUp
                Selecter++;
            if (Input.GetKeyDown(SelectDown))       //CountDown
                Selecter--;
            if (Selecter <= -1)                     //マイナス値除外
                Selecter = nLength - 1;
            
            Selecter = Selecter % nLength;          //Selecterのループ
            Next_Scene = Next_SceneIndex[Selecter]; //Next_SceneIndexの要素を取得
        }

        //Scene遷移
        if (Input.GetKeyDown(TransCode))
            MySceneManager.LoadScene(Next_Scene,false,false);
	}

    //--- Editor ------------------------------------------
#if UNITY_EDITOR
    [CustomEditor(typeof(SceneTransition))]
    [CanEditMultipleObjects]
    private class ThisEditer : Editor
    {
        public override void OnInspectorGUI()
        {
            var sceneTransition = target as SceneTransition;

            base.OnInspectorGUI();
        }
    }
#endif
}


