using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketVolume : MonoBehaviour {

    public float rocketVolume = 0.0f;
    public AudioSource audioSource;
    public AudioSource metalSource;

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }
	
	// Update is called once per frame
	void Update () {
        AudioManager manager = AudioManager.Instance;
        manager.ChangeSEVolume(audioSource, manager.SE_ROCKET, rocketVolume);
    }

    public void PlayRocketSE()
    {
        AudioManager manager = AudioManager.Instance;
        manager.PlaySE(audioSource, manager.SE_ROCKET);
    }

    public void StopRocketSE()
    {
        AudioManager manager = AudioManager.Instance;
        manager.StopSE(audioSource);
    }

    public void PlayMetalSE()
    {
        AudioManager manager = AudioManager.Instance;
        manager.PlaySEOneShot(metalSource, manager.SE_METAL);
    }
}
