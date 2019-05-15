using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataType
{
    [System.Serializable]
    public class PlayerData
    {
        public static readonly string Extension = ".player";

        public int SelectGalaxy = 0;
        public int SelectPlanet = 0;

        public int nStarCrystal = 0;
        public int nCrystal = 0;
    }

    [System.Serializable]
    public class PlanetData
    {
        public static readonly string Extension = ".planet";
    }

    [System.Serializable]
    public class CommonData
    {
        
    }
}
