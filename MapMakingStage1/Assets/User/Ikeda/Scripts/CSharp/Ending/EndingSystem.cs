using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSystem : MonoBehaviour {

    [SerializeField]
    private AudioSource audioSource;

	// Use this for initialization
	void Start ()
    {
        audioSource = this.GetComponent<AudioSource>();
        AudioManager.Instance.PlaySE(audioSource,AudioManager.Instance.BGM_END);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void NextScene()
    {
        MySceneManager.FadeInLoad(MySceneManager.Instance.Path_Title,true);
    }
}
