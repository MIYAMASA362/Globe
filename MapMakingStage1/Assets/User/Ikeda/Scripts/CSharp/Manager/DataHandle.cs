using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataType;

public static class DataHandle
{
    //アプリケーション
    private static string ApplicationPath()
    {
        return Application.dataPath;
    }

    //File検索
    public static bool FileFind(string FilePath)
    {
        return System.IO.File.Exists(ApplicationPath() + FilePath);
    }

    //Directory検索
    public static bool DirectoryFind(string DirectoryPath)
    {
        return System.IO.Directory.Exists(ApplicationPath() + DirectoryPath);
    }

    //Fileのセーブ
    public static bool Save<Type>(ref Type SaveData, string FilePath)
    {
        if (SaveData == null) return false;

        string path = ApplicationPath()+ FilePath;
        FileInfo info = new FileInfo(path);

        var json = JsonUtility.ToJson(SaveData);
        var writer = new StreamWriter(path, false);
        writer.WriteLine(json);
        writer.Flush();
        writer.Close();

        return true;
    }

    //Fileのロード
    public static bool Load<Type>(ref Type LoadData, string FilePath)
    {
        FileInfo info = new FileInfo(ApplicationPath()+ FilePath);
        if (!info.Exists || info.Length == 0) return false;

        var render = new StreamReader(info.OpenRead());
        var json = render.ReadToEnd();
        if (json == "") return false;
        LoadData = JsonUtility.FromJson<Type>(json);

        return true;
    }

    //ディレクトリの作成
    public static void Create_Directory(string DirectoryPath)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(ApplicationPath() + DirectoryPath);
        if (!directoryInfo.Exists) directoryInfo.Create();
    }

    //ディレクトリの削除
    public static void Delete_Directory(string DirectoryPath)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(ApplicationPath() + DirectoryPath);
        if (!directoryInfo.Exists) return;
    }

    //ディレクトリ内のファイル削除
    public static void Delete_DirectoryFile(string DirectoryPath)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(ApplicationPath() + DirectoryPath);
        if (!directoryInfo.Exists) return;

        foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            fileInfo.Delete();
    }

    //ディレクトリ内の特定拡張子ファイルを削除
    public static void Delete_DirectoryFile(string DirectoryPath, string Extension)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(ApplicationPath() + DirectoryPath);
        if (!directoryInfo.Exists) return;

        foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            if (fileInfo.Extension == Extension)
                fileInfo.Delete();
    }

    ////ディレクトリのデータ初期化
    //public static void Delete_LocalDirectoryData(bool CleanUp)
    //{
    //    string path = Application.dataPath + DATA_FOLDER;

    //    DirectoryInfo directory = new DirectoryInfo(path);
    //    if (!directory.Exists) return;

    //    foreach (FileInfo file in directory.GetFiles())
    //    {
    //        if (CleanUp) { file.Delete(); continue; }

    //        if (file.Extension == PlayerData.Extension || file.Extension == PlayerData.Extension + META)
    //        {
    //            PlayerData playerData = new PlayerData();
    //            Save(ref playerData, playerData.FileName());
    //            continue;
    //        }

    //        if (file.Extension == PlanetData.Extension || file.Extension == PlanetData.Extension + META)
    //        {
    //            var render = new StreamReader(file.OpenRead());
    //            var json = render.ReadToEnd();
    //            if (json == "") continue;

    //            PlanetData OldData = JsonUtility.FromJson<PlanetData>(json);
    //            PlanetData NewData = new PlanetData(OldData.Get_Name());
    //            Save(ref NewData, NewData.FileName());

    //            continue;
    //        }
    //    }
    //}
}
