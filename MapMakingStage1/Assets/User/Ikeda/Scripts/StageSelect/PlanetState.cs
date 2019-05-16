using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataType;
using UnityEngine.SceneManagement;

public class PlanetState : MonoBehaviour
{

    [Header("Planet Name")]
    [SerializeField]
    public string PlanetName = "NONE";

    [Space(8)]
    [Header("Data")]
    [SerializeField] public string DataFile = "";
    [SerializeField] private PlanetData planetData;

    private void Awake()
    {
        LoadData();
    }

    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //--- Method --------------------------------------------------------------

    public void LoadData()
    {
        planetData = new PlanetData(DataFile);

        if (DataHandle.FileFind(planetData.FileName()))
        {
            DataHandle.Load(ref planetData, planetData.FileName());
            
        }
        else
        {
            DataHandle.Save(ref planetData, planetData.FileName());
        }

        if (planetData.StageName != this.PlanetName)
        {
            planetData.StageName = this.PlanetName;
            DataHandle.Save(ref planetData, planetData.FileName());
        }
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
