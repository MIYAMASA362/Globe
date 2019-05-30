using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultRocketFire : MonoBehaviour {

    [SerializeField] private GameObject ShipFire = null;

	// Use this for initialization
	void Start ()
    {
        ShipFire.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    public void PlayFanfare()
    {
        AudioManager.Instance.FadeOutBGM();
    }

    public void PlayRocketAudio()
    {
        ShipFire.SetActive(true);
    }

}
