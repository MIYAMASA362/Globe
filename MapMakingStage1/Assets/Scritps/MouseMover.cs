using System.Collections;
using UnityEngine;

public class MouseMover : MonoBehaviour {

	public string buttonName = "Fire1";

	public float turnSpeed = 1.5f;
    public float turnSmoothing = .1f;

	float smoothX;
	float smoothY;

	public float lerp1;
	public float lerp2;

	public float smoothXvelocity = 0;
    public float smoothYvelocity = 0;


	public Transform pivot;
	public Transform body;
	public float tiltAngle;
	public float lookAngle;

	public bool canMove;
	public bool sun;

	public Camera cam;
	public float smoothness = 3;


	void Update () {

		canMove = Input.GetButton(buttonName);

		float h = Input.GetAxis("Mouse X");
		float v = Input.GetAxis("Mouse Y");

		if(canMove){
		

		if(turnSmoothing > 0)
		{
        smoothX = h;//Mathf.SmoothDamp(smoothX,h,ref smoothXvelocity,turnSmoothing);
        smoothY = v;//Mathf.SmoothDamp(smoothY,v,ref smoothYvelocity,turnSmoothing);
        }
        else
        {
         smoothX = h;
         smoothY = v;
        }
		lookAngle += smoothX * turnSpeed;
		tiltAngle += smoothY * turnSpeed;
		}

		lerp1 = Mathf.Lerp(lerp1,lookAngle,Time.deltaTime * smoothness);
		lerp2 = Mathf.Lerp(lerp2,tiltAngle,Time.deltaTime * smoothness);
		
        if(!sun){
		    body.transform.rotation = Quaternion.Euler(lerp2,lerp1,0f);
		}else{
			body.transform.rotation = Quaternion.Euler(lerp2,0f,0f);

		}
	}
}
