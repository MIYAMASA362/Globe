using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;

public class OpeningAnimationHandler : MonoBehaviour {

    public PlanetScene planetScene;
    public OpenigAnimationEvent animationEvent;
    public AroundCameraEvent aroundCamera;
    public GameObject startCharacter;
    public StateManager gameCharacter;


    // Use this for initialization
    void Start () {
        aroundCamera.Init(planetScene);
        startCharacter.SetActive(true);
        gameCharacter.gameObject.SetActive(false);
        this.gameCharacter.axisDevice.gameObject.SetActive(false);
        animationEvent.Init(startCharacter, gameCharacter);
	}
	
	// Update is called once per frame
	void Update () {
		if(planetScene.state == PlanetScene.STATE.OPENING)
        {
            animationEvent.AnimationStart();
            aroundCamera.AnimationStart();
        }
	}
}
