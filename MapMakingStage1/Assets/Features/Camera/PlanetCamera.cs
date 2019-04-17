using System.Collections;
using UnityEngine;

namespace FrameWork.Camera
{
    public class PlanetCamera : MonoBehaviour
    {
        public string buttonName = "Fire1";

        public float turnSpeed = 1.5f;
        public float smoothness = 3;

        private float lerp1;
        private float lerp2;
        public float tiltAngle;
        public float lookAngle;


        public bool canMove;


        void Update()
        {
            canMove = Input.GetButton(buttonName);

            float smoothX = Input.GetAxis("Mouse X");
            float smoothY = Input.GetAxis("Mouse Y");

            if (canMove)
            {
                lookAngle += smoothX * turnSpeed;
                tiltAngle += smoothY * turnSpeed;
            }

            lerp1 = Mathf.Lerp(lerp1, lookAngle, Time.deltaTime * smoothness);
            lerp2 = Mathf.Lerp(lerp2, tiltAngle, Time.deltaTime * smoothness);

            transform.rotation = Quaternion.Euler(lerp2, lerp1, 0f);
        }
    }
}