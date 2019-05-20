using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataType;
using UnityEngine.SceneManagement;

public class PlanetState : MonoBehaviour
{
    public enum eGalaxyNum
    {
        Galaxy1 = 0,
        Galaxy2 = 1,
        Galaxy3 = 2,
        Galaxy4 = 3
    }

    public enum ePlanetNum
    {
        Planet1 = 0,
        Planet2 = 1,
        Planet3 = 2,
        Planet4 = 3,
        Planet5 = 4
    }

    [Header("PlanetState")]
    [SerializeField] public eGalaxyNum GalaxyNum = eGalaxyNum.Galaxy1;
    [SerializeField] public ePlanetNum PlanetNum = ePlanetNum.Planet1;

    [Space(4)]
    [Header("Data")]
    [SerializeField] public string DataFile = "NONE";
    [SerializeField] private PlanetData planetData;

    [HideInInspector] public string PlanetName;
    

    private void Awake()
    {
        LoadData();
    }

    //--- Method --------------------------------------------------------------

    public void LoadData()
    {
        planetData = new PlanetData(DataFile);

        if (DataHandle.FileFind(planetData.FileName()))
            DataHandle.Load(ref planetData, planetData.FileName());
    }

    public int CrystalNum()
    {
        return planetData.IsGet_Crystal ? 1:0;
    }

    public int StarCrystalNum()
    {
        return planetData.IsGet_StarCrystal ? 1 : 0;
    }

    public int Crystal_ReaminingNum()
    {
        return 1 - CrystalNum();
    }

    public int StarCrystal_ReaminingNum()
    {
        return 1 - StarCrystalNum();
    }
}
