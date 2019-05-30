using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultEndEvent : MonoBehaviour
{
    private PlanetScene planetScene;
    private PlanetResult planetResult;
    public ParticleSystem fire;
    public GameObject resultCamera;
    public AudioSource audioSource;

	// Use this for initialization
	void Start ()
    {
        planetScene = PlanetScene.Instance;
        planetResult = planetScene.GetComponent<PlanetResult>();
        fire.Stop();
        resultCamera.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetResultCamera()
    {
        resultCamera.SetActive(true);
    }

    public void PlayFire()
    {
        fire.Play();
    }

    public void NextScene()
    {
        planetScene.NextScene();
    }

    public void PrintResult()
    {
        planetResult.Print();
    }

    public void IsInputEnable()
    {
        planetResult.IsInputEnable();
    }

    public void FadeBGM()
    {
        AudioManager.Instance.FadeOutBGM();
    }

    public void PlayFanfare()
    {
        AudioManager.Instance.PlaySEOneShot(audioSource, AudioManager.Instance.SE_FANFARE);
    }
}
