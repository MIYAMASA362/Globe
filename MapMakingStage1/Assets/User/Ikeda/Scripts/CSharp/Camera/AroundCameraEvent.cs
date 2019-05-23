using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AroundCameraEvent : MonoBehaviour {

    [SerializeField] private PlanetScene planetScene;

    public void EndOpening()
    {
        planetScene.EndOpening();
        planetScene.state = PlanetScene.STATE.MAINGAME;
    }
}
