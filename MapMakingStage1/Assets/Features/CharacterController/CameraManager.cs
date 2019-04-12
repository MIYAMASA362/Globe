using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
	public class CameraManager : MonoBehaviour
	 {


		public bool lockOn;

		public float followSpeed = 9;
		public float mouseSpeed = 2;
		// public float controllerSpeed = 7;

		public Transform target;

		public Transform pivot;
		public Transform camTrans;

		float turnSmoothing = .1f;
		public float minAngle = -35;
		public float maxAngle =  35;

		float smoothX;
		float smoothY;
        float smoothXvelocity;
		float smoothYvelocity; 
		public float lookAngle;
		public float tiltAngle;

		public float z;

		public static CameraManager singleton;

		public GameObject body;

		public Vector3 gravityDirection;

		
		//------------------------------------------
		//------------------------------------------
		public void Init(Transform t)
		{

			Debug.Log("Init");
			target = t;

			camTrans = Camera.main.transform; 
			pivot = camTrans.parent;
		}
		//------------------------------------------
		void LateUpdate(){

			float speed = Time.deltaTime* followSpeed;
			Vector3 targetPosition  = Vector3.Lerp(transform.position,target.position,speed);
			transform.position = targetPosition;



            Vector3 planetPosition = RotationManager.Instance.planetTransform.position;

			gravityDirection = (transform.position - planetPosition);
			gravityDirection.Normalize();

			Quaternion q = Quaternion.FromToRotation(-transform.up, -gravityDirection);
            q = q * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, q, 1);

			 // transform.up -= (transform.up - gravityDirection);

			 

		}
		public void Tick(float d)
		{




			float h = Input.GetAxis("Mouse X");
			float v = Input.GetAxis("Mouse Y");

			float targetSpeed = mouseSpeed;

			/*
			float c_h = Input.GetAxis("RightAxis X");
			float c_v = Input.GetAxis(LeftAxis X");

			if(c_h != 0 || c_v != 0)
			{
				h = c_;;
				v = c_v;
				targetSpeed = controllerSpeed;
			}
			*/
			FollowTarget(d);
			HandleRotations(d,v,h,targetSpeed);
		}
		//------------------------------------------
		void FollowTarget(float d)
		{
			float speed = d * followSpeed;
			Vector3 targetPosition  = Vector3.Lerp(transform.position,target.position,speed);
			transform.position = targetPosition;

		}
		//------------------------------------------
		void HandleRotations(float d ,float v,float h,float targetSpeed)
		{
			if(turnSmoothing > 0)
			{
				smoothX = Mathf.SmoothDamp(smoothX,h,ref smoothXvelocity, turnSmoothing);
				smoothY = Mathf.SmoothDamp(smoothY,v,ref smoothYvelocity, turnSmoothing);
			}
			else
			{
				smoothX = h;
				smoothY = v;
			}

			if(lockOn)
			{


			}

			lookAngle += smoothX * targetSpeed;
			body.transform.localRotation= Quaternion.Euler(0,lookAngle,0);

			tiltAngle -= smoothY * targetSpeed;
			tiltAngle = Mathf.Clamp(tiltAngle,minAngle,maxAngle);
			pivot.localRotation = Quaternion.Euler(tiltAngle,0,0);


			
			

		}
		//------------------------------------------
		void Awake()
	    {
		singleton = this;
	    }
    }
}//end