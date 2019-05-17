using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DataType
{
    [System.Serializable]
    public class PlayerData
    {
        public static readonly string Extension = ".player";

        public bool IsContinue = false;

        public int SelectGalaxy = 0;
        public int SelectPlanet = 0;

        public int nStarCrystal = 0;
        public int nCrystal = 0;

        public string FileName()
        {
            return "Player" + Extension;
        }
    }

    [System.Serializable]
    public class PlanetData
    {
        public static readonly string Extension = ".planet";
        private string Name = "NONE";

        public bool IsGet_StarCrystal = false;
        public bool IsGet_Crystal = false;
        public bool IsClear = false;

        public PlanetData(string Name)
        {
            this.Name = Name;
        }

        public string FileName()
        {
            return Name + Extension;
        }

        public string Get_Name() 
        {
            return Name;
        }

    }

    [System.Serializable]
    public class CommonData
    {
        
    }
}
