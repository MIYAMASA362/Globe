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

    [Header("SE")]
    [SerializeField]
    AudioSource SelectSound;


    //--- MonoBehavior ------------------------------------

    public void Awake()
    {
        

    }

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

    public void PlayAudio_Success()
    {
        if (SelectSound.isPlaying)
            SelectSound.Stop();
        AudioManager.Instance.PlaySE(SelectSound,AudioManager.Instance.SE_SUCCESS);
    }

    public void PlayAudio_Select()
    {
        if (SelectSound.isPlaying)
            SelectSound.Stop();
        AudioManager.Instance.PlaySE(SelectSound, AudioManager.Instance.SE_SELECT);
    }

    public void PlayAudio_Return()
    {
        if (SelectSound.isPlaying)
            SelectSound.Stop();
        AudioManager.Instance.PlaySE(SelectSound, AudioManager.Instance.SE_RETURN);
    }

}