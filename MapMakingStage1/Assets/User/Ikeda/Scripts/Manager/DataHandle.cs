using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataType;

public static class DataHandle
{
    private const string DATA_FOLDER = "/Resources/local";

    private static string DataPath(string FileName)
    {
        return Application.dataPath + DATA_FOLDER + "/" + FileName;
    }

    public static bool FileFind(string FileName)
    {
        return System.IO.File.Exists(DataPath(FileName));
    }

    public static bool Save<Type>(ref Type SaveData,string FileName)
    {
        if (SaveData == null) return false;
        
        string path = DataPath(FileName);
        FileInfo info = new FileInfo(path);

        var json = JsonUtility.ToJson(SaveData);
        var writer = new StreamWriter(path,false);
        writer.WriteLine(json);
        writer.Flush();
        writer.Close();

        return true;
    }

    public static bool Load<Type>(ref Type LoadData,string FileName)
    {
        FileInfo info = new FileInfo(DataPath(FileName));
        if (!info.Exists || info.Length == 0) return false;

        var render = new StreamReader(info.OpenRead());
        var json = render.ReadToEnd();
        if (json == "") return false;
        LoadData = JsonUtility.FromJson<Type>(json);

        return true;
    }

    public static void Create_LocalDirectory()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + DATA_FOLDER);
        if (!directoryInfo.Exists) directoryInfo.Create();
    }

    public static void Delete_LocalDirectoryData()
    {
        string path = Application.dataPath + DATA_FOLDER;

        DirectoryInfo directory = new DirectoryInfo(path);
        if (!directory.Exists) return;

        foreach (FileInfo file in directory.GetFiles())
        {
            if (file.Extension != PlayerData.Extension || file.Extension != PlayerData.Extension + ".meta")
            {
                file.Attributes = FileAttributes.Normal;
                file.Delete();
            }
        }
    }
}
