using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork.Camera
{
    public class CharacterCamera : MonoBehaviour
    {
        public Transform followTarget;
        public Transform rotationPivot;
        public Transform cameraTransform;

        public float followSpeed = 6;
        public float mouseSpeed = 3;
        
        public float minAngle = -5;
        public float maxAngle = 50;
        public float distance = 1.6f;

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
            cameraTransform.localPosition = new Vector3(0.0f, 0.0f, -distance);
        }

        //------------------------------------------
        public void Init(Transform t)
        {
            Debug.Log("Init");
            followTarget = t;
        }
        //------------------------------------------
        void LateUpdate()
        {
            float speed = Time.deltaTime * followSpeed;
            Vector3 targetPosition = Vector3.Lerp(transform.position, followTarget.position, speed);
            transform.position = targetPosition;

            Vector3 planetPosition = RotationManager.Instance.planetTransform.position;
            Vector3 gravityDirection = (planetPosition - transform.position);
            gravityDirection.Normalize();

            Quaternion q = Quaternion.FromToRotation(-transform.up, gravityDirection);
            q = q * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, q, 1);
        }
        public void Tick(float d)
        {
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");

            float targetSpeed = mouseSpeed;

            FollowTarget(d);
            HandleRotations(d, v, h, targetSpeed);
        }
        //------------------------------------------
        void FollowTarget(float d)
        {
            float speed = d * followSpeed;
            Vector3 targetPosition = Vector3.Lerp(transform.position, followTarget.position, speed);
            transform.position = targetPosition;

        }
        //------------------------------------------
        void HandleRotations(float d, float v, float h, float targetSpeed)
        {
            smoothX = Mathf.SmoothDamp(smoothX, h, ref smoothXvelocity, turnSmoothing);
            smoothY = Mathf.SmoothDamp(smoothY, v, ref smoothYvelocity, turnSmoothing);

            lookAngle += smoothX * targetSpeed;
            tiltAngle -= smoothY * targetSpeed;
            tiltAngle = Mathf.Clamp(tiltAngle, minAngle, maxAngle);

            rotationPivot.localRotation = Quaternion.Euler(tiltAngle, lookAngle, 0);
        }
    }
}//end