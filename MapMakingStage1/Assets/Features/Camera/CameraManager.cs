using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork.Camera
{
    public class CameraManager : Singleton<CameraManager>
    {
        public bool isChange;
        public GameObject planetCamera;
        public GameObject characterCamera;

        void Update()
        {
            if (Input.GetButtonDown(InputManager.View_Swith))
            {
                isChange = !isChange;
                planetCamera.SetActive(!isChange);
                characterCamera.SetActive(isChange);
            }
        }
    }
}
