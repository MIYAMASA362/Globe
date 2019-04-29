using System.Collections;
using UnityEngine;

namespace FrameWork.Camera
{
    public class PlanetCamera : MonoBehaviour
    {
        public Transform rotationPivot;
        public Transform cameraTransform;

        public float upSpeed = 3.0f;
        public float mouseSpeed = 1.5f;

        public float minAngle = -80f;
        public float maxAngle = 80f;
        public float distance = 13f;

        public float lookAngle;
        public float tiltAngle;

        float turnSmoothing = .1f;
        float smoothX;
        float smoothY;
        float smoothXvelocity;
        float smoothYvelocity;

        private void Start()
        {
            rotationPivot.transform.localPosition = Vector3.zero;
            cameraTransform.localPosition = new Vector3(0.0f, 0.0f, distance);
        }


        private void Update()
        {
            Tick(Time.deltaTime);
        }

        //------------------------------------------
        void LateUpdate()
        {
            Quaternion q = Quaternion.FromToRotation(transform.up, FlagManager.Instance.flagTransform.up);
            q = q * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * upSpeed);
        }

        public void Tick(float d)
        {
            float h = Input.GetAxis(InputManager.Camera_Horizontal);
            float v = Input.GetAxis(InputManager.Character_Camera_Vertical);

            float targetSpeed = mouseSpeed;

            HandleRotations(d, v, h, targetSpeed);
        }
        
        //------------------------------------------
        void HandleRotations(float d, float v, float h, float targetSpeed)
        {
            smoothX = Mathf.SmoothDamp(smoothX, h, ref smoothXvelocity, turnSmoothing);
            smoothY = Mathf.SmoothDamp(smoothY, v, ref smoothYvelocity, turnSmoothing);

            lookAngle += smoothX * targetSpeed;
            tiltAngle += smoothY * targetSpeed;
            tiltAngle = Mathf.Clamp(tiltAngle, minAngle, maxAngle);

            rotationPivot.localRotation = Quaternion.Euler(tiltAngle, lookAngle, 0);
        }
    }
}