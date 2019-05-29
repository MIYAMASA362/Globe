using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultRocketSE : MonoBehaviour {

    private AudioSource RocketAudioSource;
    private bool IsPlay;
    private bool IsVolumeFade;

	// Use this for initialization
	void Start ()
    {
        RocketAudioSource = this.GetComponent<AudioSource>();
        RocketAudioSource.volume = RocketAudioSource.volume * AudioManager.Instance.SE_masterVolume;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!IsPlay) return;
        if (RocketAudioSource.isPlaying) return;
        RocketAudioSource.Play();
	}

    public void PlayRocketAudio()
    {
        RocketAudioSource.Play();
    }

    public void StopRocketAudio()
    {
        RocketAudioSource.Stop();
    }

    public void VolumeFade()
    {

    }
}
