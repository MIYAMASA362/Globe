using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataType
{
    public static class DirectoryPath
    {
        public const string local = "/Resources/local";
        public const string common = local + "/Common";
        public const string player = local + "/Player";
        public const string planets = local+"/Planets";
    }

    [System.Serializable]
    public class DataBase
    {
        private string Name = "";
        private string Extension = "";

        public DataBase(string Name,string Extension)
        {
            this.Extension = Extension;
            this.Name = Name;
        }

        private string FileName()
        {
            return Name + Extension;
        }

        public virtual string FilePath()
        {
            return FileName();
        }
    }


    [System.Serializable]
    public class PlayerData:DataBase
    {
        public bool IsContinue = false;
        public int SelectGalaxy = 0;
        public int SelectPlanet = 0;

        public int GetCrystalNum = 0;
        public int GetStarCrystalNum = 0;

        public PlayerData() : base("Player", ".player") { }

        public override string FilePath()
        {
            return DirectoryPath.player +"/"+ base.FilePath();
        }
    }

    [System.Serializable]
    public class PlanetData :DataBase
    {
        public bool IsGet_StarCrystal = false;
        public bool IsGet_Crystal = false;
        public bool IsClear = false;

        public PlanetData(string Name):base(Name,".planet") { }

        public override string FilePath()
        {
            return DirectoryPath.planets +"/" + base.FilePath();
        }

        public bool IsComplate()
        {
            return IsGet_StarCrystal && IsGet_Crystal;
        }
    }

    [System.Serializable]
    public class CommonData:DataBase
    {
        public float BGM_Volume = 100f;
        public float SE_Volume = 100f;
        public bool IsCameraReverseVertical = false;
        public bool IsCameraReverseHorizontal = false;
        public bool IsVibration = true;
        public float fVibration = 100f;
        public bool IsSuport = true;

        public CommonData():base("Common",".data") { }

        public override string FilePath()
        {
            return DirectoryPath.common + "/" + base.FilePath();
        }
    }
}
