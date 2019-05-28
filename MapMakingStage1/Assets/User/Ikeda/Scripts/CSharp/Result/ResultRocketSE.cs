using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultRocketSE : MonoBehaviour {

    private AudioSource RocketAudioSource;
    private float OldVolume;

	// Use this for initialization
	void Start ()
    {
        RocketAudioSource = this.GetComponent<AudioSource>();
        OldVolume = RocketAudioSource.volume;
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    public void PlayRocketAudio()
    {
        RocketAudioSource.Play();
    }

    public void StopRocketAudio()
    {
        RocketAudioSource.Stop();
    }
}
