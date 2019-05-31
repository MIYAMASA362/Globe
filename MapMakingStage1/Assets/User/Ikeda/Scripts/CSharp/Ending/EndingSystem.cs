using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSystem : MonoBehaviour {

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    GameObject Player;

	// Use this for initialization
	void Start ()
    {
        audioSource = this.GetComponent<AudioSource>();
        AudioManager.Instance.PlaySE(audioSource,AudioManager.Instance.BGM_END);
	}
	
	// Update is called once per frame
	void Update ()
    {
        float Axis = Input.GetAxis(InputManager.Horizontal);
        Player.transform.rotation = Quaternion.AngleAxis(Axis*10f,Vector3.forward) * Player.transform.rotation;
	}

    public void NextScene()
    {
        MySceneManager.FadeInLoad(MySceneManager.Instance.Path_Title,true);
    }
}
