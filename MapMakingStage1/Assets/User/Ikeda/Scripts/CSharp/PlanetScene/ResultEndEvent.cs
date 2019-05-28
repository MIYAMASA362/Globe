using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultEndEvent : MonoBehaviour
{
    private PlanetScene planetScene;
    private PlanetResult planetResult;

	// Use this for initialization
	void Start ()
    {
        planetScene = PlanetScene.Instance;
        planetResult = planetScene.GetComponent<PlanetResult>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void NextScene()
    {
        planetScene.NextScene();
    }

    public void PrintResult()
    {
        planetResult.Print();
    }

    public void UnLoadPlayer()
    {
        planetResult.UnLoadPlayer();
    }

    public void HideAxisDevice()
    {
        planetResult.HideAxisDevice();
    }

    public void IsInputEnable()
    {
        planetResult.IsInputEnable();
    }
}
