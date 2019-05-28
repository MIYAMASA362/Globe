using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultEndEvent : MonoBehaviour
{

    [SerializeField] private PlanetScene planetScene;
    private PlanetResult planetResult;

	// Use this for initialization
	void Start ()
    {
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
}
