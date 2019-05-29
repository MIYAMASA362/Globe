using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultRocketSE : MonoBehaviour {

    private AudioSource RocketAudioSource;
    [SerializeField] private GameObject ShipFire = null;
    private float OldVolume;

	// Use this for initialization
	void Start ()
    {
        RocketAudioSource = this.GetComponent<AudioSource>();
        RocketAudioSource.loop = true;
        OldVolume = RocketAudioSource.volume;
        ShipFire.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {

	}

    public void PlayRocketAudio()
    {
        ShipFire.SetActive(true);
        RocketAudioSource.Play();
    }

    public void StopRocketAudio()
    {
        RocketAudioSource.Stop();
    }
}
