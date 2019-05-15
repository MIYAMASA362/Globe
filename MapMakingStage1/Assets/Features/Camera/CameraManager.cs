using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraManager : Singleton<CameraManager>
{
    public bool isChange;
    public Camera mainCamera;
    public PlanetCamera planetCamera;
    public CharacterCamera characterCamera;

    void Update()
    {
        if (Input.GetButtonDown(InputManager.View_Swith))
        {
            isChange = !isChange;
            planetCamera.gameObject.SetActive(!isChange);
            characterCamera.gameObject.SetActive(isChange);
        }
    }
}

