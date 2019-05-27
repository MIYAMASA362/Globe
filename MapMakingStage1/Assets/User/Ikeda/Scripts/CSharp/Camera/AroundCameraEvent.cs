using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AroundCameraEvent : MonoBehaviour {

    public Animator anim;
    private PlanetScene planetScene;

    public void Init(PlanetScene planetScene)
    {
        this.planetScene = planetScene;
        anim.enabled = false;
    }

    public void AnimationStart()
    {
        anim.enabled = true;
    }

    public void EndOpening()
    {
        planetScene.SetState(PlanetScene.STATE.MAINGAME);
        PlanetCamera planetCamera = CameraManager.Instance.planetCamera;

        planetCamera.transform.position = this.transform.position;
        planetCamera.transform.rotation = this.transform.rotation;
        CameraManager.Instance.SetStart();

        this.gameObject.SetActive(false);
    }
}
