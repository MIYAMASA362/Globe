using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;

public class OpeningAnimationHandler : MonoBehaviour {

    public OpenigAnimationEvent animationEvent;
    public GameObject startCharacter;
    public StateManager gameCharacter;


    // Use this for initialization
    void Start () {
        startCharacter.SetActive(true);
        gameCharacter.gameObject.SetActive(false);
        animationEvent.Init(startCharacter, gameCharacter);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
