using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DataType;

public class DataManager : Singleton<DataManager>
{
    //--- Attribute ---------------------------------------

    [Header("Player Status")]
    [SerializeField] public PlayerData playerData = null;



    //--- MonoBehaviour -----------------------------------

    private void Awake()
    {
        DontDestroyOnLoad(this);

        //保存先のディレクトリが存在するかの確認
        DataHandle.Create_LocalDirectory();

        playerData = new PlayerData();
        Load_PlayerData();
        //Save_PlayerData();
    }

    private void Start()
    {

    }

    private void Update()
    {
        
    }

    //--- Method ------------------------------------------

    //--- Player ---

    public PlayerData GetPlayerData()
    {
        return playerData;
    }

    public bool Load_PlayerData()
    {
        if (!DataHandle.FileFind(playerData.FileName())) return false;
        DataHandle.Load(ref playerData, playerData.FileName());
        return true;
    }

    public void Save_PlayerData()
    {
        DataHandle.Save(ref playerData,playerData.FileName());
    }

    public void Reset_DataState()
    {
        DataHandle.Delete_LocalDirectoryData();

        playerData = new PlayerData();
        playerData.IsContinue = true;
        Save_PlayerData();
    }
}