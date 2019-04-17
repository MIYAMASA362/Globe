using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork.Camera
{
    public class CameraManager : Singleton<CameraManager>
    {
        public bool isChange;
        public KeyCode keycode;
        public GameObject planetCamera;
        public GameObject characterCamera;

        void Update()
        {
            if (Input.GetKeyDown(keycode))
            {
                isChange = !isChange;
                planetCamera.SetActive(!isChange);
                characterCamera.SetActive(isChange);
            }
        }
    }
}
