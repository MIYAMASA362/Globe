using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceAlign : MonoBehaviour {

    RaycastHit hit;
    public Vector3 surfaceNormal;
	public Vector3 surfaceNormal2;

	public Vector3 surfaceNormalCombined;

	public bool doubleNormal;
	public Transform frontLeftOrigin;
	public Transform frontRightOrigin;
	public Transform backLeftOrigin;
	public Transform backRightOrigin;



    Vector3 forwardRelativeToSurfaceNormal;//For Look Rotation
	Quaternion targetRotation;
	public LayerMask ignoreLayers;
	public float toGround = 2;
	public float smoothness = 10;

	public Vector3 offset;
	public Vector3 upDir;

	public Vector3 frLpoint1;
	public Vector3 frRpoint2;
	public Vector3 bLpoint3;
	public Vector3 bRpoint4;


	public bool frontL;
	public bool frontR;
	public bool backR;
	public bool backL;

	public float raycastDistance = 5;
    
  
    void FixedUpdate () {
        CharacterFaceRelativeToSurface ();
    }
 

    private void CharacterFaceRelativeToSurface()
    {
    

		Vector3 frontLeft  = frontLeftOrigin.position + (Vector3.up * toGround);


		if(doubleNormal)
		{
		//surfaceNormalCombined = (surfaceNormal + surfaceNormal2).normalized;
		surfaceNormalCombined = upDir;//(surfaceNormal + surfaceNormal2).normalized;

	
		}
		else
		{
			surfaceNormalCombined  = surfaceNormal ;

		}


		if(frontL && frontR){
		   upDir = (Vector3.Cross(bRpoint4 - Vector3.up, bLpoint3 - Vector3.up) +
              Vector3.Cross(bLpoint3 - Vector3.up, frLpoint1 - Vector3.up) +
              Vector3.Cross(frLpoint1 - Vector3.up, frRpoint2 - Vector3.up) +
              Vector3.Cross(frRpoint2 - Vector3.up, bRpoint4 - Vector3.up)
             ).normalized;
		}else{
			upDir  = surfaceNormal ;

		}


		

        if (Physics.Raycast(frontLeft, -Vector3.up, out hit, raycastDistance,ignoreLayers))
        {
            surfaceNormal = hit.normal; 
			frLpoint1 = hit.point;
			frontL = true;

            forwardRelativeToSurfaceNormal = Vector3.Cross(transform.right, surfaceNormalCombined);
             targetRotation = Quaternion.LookRotation(forwardRelativeToSurfaceNormal, surfaceNormalCombined); 
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smoothness); 
        }
		else
		{
			frontL = false;

		}
		//----------------------------------------------------------------------
		Vector3 frontRight  = frontRightOrigin.position + (Vector3.up * toGround);

		 if (Physics.Raycast(frontRight, -Vector3.up, out hit, raycastDistance,ignoreLayers))
        {
			frRpoint2 = hit.point;
			frontR = true;
		}
		else
		{
			frontR = false;
		}
		//----------------------------------------------------------------------
		Vector3 backRight  = backRightOrigin.position + (Vector3.up * toGround);

		 if (Physics.Raycast(backRight, -Vector3.up, out hit, raycastDistance,ignoreLayers))
        {
			bLpoint3 = hit.point;
			backR = true;
		}else{
			backR = false;
		}
		//----------------------------------------------------------------------
		Vector3 backLeft  = backLeftOrigin.position + (Vector3.up * toGround);

		 if (Physics.Raycast(backLeft, -Vector3.up, out hit, raycastDistance,ignoreLayers))
        {
			bRpoint4 = hit.point;
			backL = true;
		}else{
			backL = false;
		}

		Debug.DrawRay(frontRight, Vector3.up);
		Debug.DrawRay(frontLeft, Vector3.up);
		Debug.DrawRay(backLeft, Vector3.up);
		Debug.DrawRay(backRight, Vector3.up);
    }
}
