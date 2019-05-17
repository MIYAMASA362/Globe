using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraManager : Singleton<CameraManager>
{
    public bool isChange;
    public PlanetCamera planetCamera;
    public CharacterCamera characterCamera;

    private bool onCharacterCamera = false;

    private void Start()
    {
        isChange = true;
    }

    void Update()
    {
        if (Input.GetButtonDown(InputManager.View_Swith))
        {
            if(!planetCamera.gameObject.activeInHierarchy)
            {
                Vector3 dir = -characterCamera.cameraTransform.forward;
                planetCamera.transform.rotation = characterCamera.cameraTransform.rotation;
                planetCamera.transform.position = RotationManager.Instance.planetTransform.position + dir * planetCamera.distance;

                isChange = !isChange;
            }
            else
            {
                characterCamera.tiltAngle = 38f;
                onCharacterCamera = true;
                planetCamera.SetTarget(characterCamera.followTarget.position, Time.deltaTime * 15);
            }
        }
        if(onCharacterCamera && !planetCamera.isMoveTarget)
        {
            isChange = !isChange;
            onCharacterCamera = false;
            
        }

        planetCamera.gameObject.SetActive(!isChange);
        characterCamera.gameObject.SetActive(isChange);
    }
}

