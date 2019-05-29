using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultEndEvent : MonoBehaviour
{
    private PlanetScene planetScene;
    private PlanetResult planetResult;
    [SerializeField] private ResultRocketSE rocketSE;

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

    public void PlayFanfare()
    {
        rocketSE.PlayFanfare();
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

    public void PlayRocketAudio()
    {
        rocketSE.PlayRocketAudio();
    }
}
