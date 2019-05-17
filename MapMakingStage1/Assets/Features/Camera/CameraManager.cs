using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraManager : Singleton<CameraManager>
{
    public bool isChange;
    public PlanetCamera planetCamera;
    public CharacterCamera characterCamera;

    void Update()
    {
        if (Input.GetButtonDown(InputManager.View_Swith))
        {
            if(!planetCamera.gameObject.activeInHierarchy)
            {
                Vector3 dir = -characterCamera.cameraTransform.forward;
                planetCamera.transform.rotation = characterCamera.cameraTransform.rotation;
                planetCamera.transform.position = RotationManager.Instance.planetTransform.position + dir * planetCamera.distance;
            }
            else
            {
                characterCamera.tiltAngle = 38f;
            }

            isChange = !isChange;
            planetCamera.gameObject.SetActive(!isChange);
            characterCamera.gameObject.SetActive(isChange);
        }
    }
}

