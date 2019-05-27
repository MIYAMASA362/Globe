using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;

public class OpeningAnimationHandler : MonoBehaviour {

    private PlanetScene planetScene;
    public StateManager gameCharacter;
    public GameObject startCharacter;
    public OpenigAnimationEvent animationEvent;
    public AroundCameraEvent aroundCamera;


    // Use this for initialization
    void Start () {
        planetScene = PlanetScene.Instance;

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
