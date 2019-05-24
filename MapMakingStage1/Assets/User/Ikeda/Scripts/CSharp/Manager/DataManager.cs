using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataType;

public class DataManager : Singleton<DataManager>
{
    //--- Attribute ---------------------------------------

    [Header("Player Status")]
    [SerializeField] public PlayerData playerData = null;

    [Space(10)]
    [Header("Common Data")]
    [SerializeField] public CommonData commonData = null;

    //--- MonoBehaviour -----------------------------------

    private void Awake()
    {
        DontDestroyOnLoad(this);

        //ローカルのディレクトリ作成
        Create_LocalDirectory();

        //データ作成
        playerData = new PlayerData();
        commonData = new CommonData();

        //プレイヤーデータの設定
        if (DataHandle.FileFind(playerData.FilePath()))
            DataHandle.Load(ref playerData, playerData.FilePath());
        else
            DataHandle.Save(ref playerData, playerData.FilePath());

        //共通データの設定
        if (DataHandle.FileFind(commonData.FilePath()))
            DataHandle.Load(ref commonData, commonData.FilePath());
        else
            DataHandle.Save(ref commonData, commonData.FilePath());
    }

    //--- Method ------------------------------------------

    //ディレクトリを設定
    public void Create_LocalDirectory()
    {
        //各ディレクトリの作成
        DataHandle.Create_Directory(DataType.DirectoryPath.local);
        DataHandle.Create_Directory(DataType.DirectoryPath.common);
        DataHandle.Create_Directory(DataType.DirectoryPath.planets);
        DataHandle.Create_Directory(DataType.DirectoryPath.player);
    }

    //ゲームデータの初期化
    public void Reset_GameData()
    {
        //Planetsのデータを削除
        if (DataHandle.DirectoryFind(DataType.DirectoryPath.planets))
            DataHandle.Delete_DirectoryFile(DataType.DirectoryPath.planets);
        
        //Playerのデータを初期化
        if (DataHandle.DirectoryFind(DataType.DirectoryPath.player))
            if (DataHandle.FileFind(this.playerData.FilePath()))
            {
                this.playerData = new PlayerData();
                this.playerData.IsContinue = true;
                DataHandle.Save(ref this.playerData, this.playerData.FilePath());
            }
    }


    //--- Player Data -----------------------------------------------

    public void PlayerData_Load()
    {
        if(DataHandle.FileFind(playerData.FilePath()))
            DataHandle.Load(ref playerData,playerData.FilePath());
    }

    public void PlayerData_Save()
    {
        DataHandle.Save(ref playerData,playerData.FilePath());
    }

    public void PlayerData_SetSelect(int SelectGalaxy,int SelectPlanet)
    {
        playerData.SelectGalaxy = SelectGalaxy;
        playerData.SelectPlanet = SelectPlanet;
    }

    //--- Common Data -----------------------------------------------

    public void CommonData_Save()
    {
        DataHandle.Save(ref commonData,commonData.FilePath());
    }

    public void CommonData_Load()
    {
        if (DataHandle.FileFind(commonData.FilePath()))
            DataHandle.Load(ref commonData, commonData.FilePath());
        else
            DataHandle.Save(ref commonData, commonData.FilePath());
    }

    public void CommonData_ReSet()
    {
        commonData = new DataType.CommonData();
    }
}