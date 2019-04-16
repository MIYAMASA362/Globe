using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class SceneBase : MonoBehaviour
{
    //--- Attribute ---------------------------------------

    //--- private -------------------------------
    [Header("State")]
    [SerializeField,Tooltip("このSceneがPause画面を使うか")]
    private bool IsPausing = true;

    //--- MonoBehavior ------------------------------------

    public virtual void Start ()
    {
        
	}

    public virtual void Update()
    {
        OnPause();
    }

    //--- Method ------------------------------------------

    //Pause画面
    public void OnPause()
    {
        if (IsPausing) MySceneManager.Pause(!MySceneManager.bPausing);
    }

    ////Scene選択切り替え
    //public void SelectChange()
    //{
    //    //Next_Sceneへの設定がある時
    //    if (nLength != 0)
    //    {
    //        if (Input.GetKeyDown(SelectUp))   Selecter++; //CountUp
    //        if (Input.GetKeyDown(SelectDown)) Selecter--; //CountDown
    //        if (Selecter <= -1) Selecter = nLength - 1;   //マイナス値除外

    //        Selecter    = Selecter % nLength;             //Selecterのループ
    //        Next_Scene  = Next_SceneIndex[Selecter];      //Next_SceneIndexの要素を取得
    //    }
    //}

}