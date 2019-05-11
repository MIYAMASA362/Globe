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
        if(IsPausing) MySceneManager.Instance.LoadBack_Pause();
    }

    public virtual void Update()
    {
        OnPause();
    }

    //--- Method ------------------------------------------

    //Pause画面
    public void OnPause()
    {
        if (IsPausing) MySceneManager.Pause(!MySceneManager.IsPausing);
    }

}