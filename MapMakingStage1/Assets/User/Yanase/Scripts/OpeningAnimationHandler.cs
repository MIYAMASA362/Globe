using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;

public class OpeningAnimationHandler : MonoBehaviour {

    private PlanetScene planetScene;
    public StartScript start;
    public StateManager gameCharacter;
    public GameObject startCharacter;
    public OpenigAnimationEvent animationEvent;
    public AroundCameraEvent aroundCamera;


    // Use this for initialization
    void Start () {
        planetScene = PlanetScene.Instance;
        if (planetScene.skipOpening)
        {
            this.gameObject.SetActive(false);
            return;
        }

        transform.position = RotationManager.Instance.planetTransform.position;
        transform.rotation = start.transform.rotation;

        aroundCamera.Init(planetScene);
        startCharacter.SetActive(true);
        gameCharacter.gameObject.SetActive(false);
        gameCharacter.axisDevice.gameObject.SetActive(false);
        animationEvent.Init(startCharacter, gameCharacter);
	}

    // Update is called once per frame
    void Update()
    {
        if (planetScene.skipOpening) return;

        if (planetScene.state == PlanetScene.STATE.OPENING)
        {
            animationEvent.AnimationStart();
            aroundCamera.AnimationStart();
        }
    }
}
