using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceWalker : MonoBehaviour {
 
    public Transform LookTransform;
 
    public float speed = 6.0f;
    public float maxVelocityChange = 10.0f;
    public float jumpForce = 5.0f;
    public float GroundHeight = 1.1f;
    private float xRotation;
    private float yRotation;
	public Rigidbody rigidbody;

    // Use this for initialization
    void Start()
    {

    }
 
    // Update is called once per frame
    void FixedUpdate () {
 
      //  RaycastHit groundedHit;
      //  bool grounded = Physics.Raycast(transform.position, -transform.up, out groundedHit, GroundHeight);
 
       // if (grounded)
       // {
            // Calculate how fast we should be moving
            Vector3 forward = Vector3.Cross(transform.up, -LookTransform.right).normalized;
            Vector3 right = Vector3.Cross(transform.up, LookTransform.up).normalized;
            Vector3 targetVelocity = (forward * Input.GetAxis("Vertical") + right * Input.GetAxis("Horizontal")) * speed;
 
            Vector3 velocity = transform.InverseTransformDirection(rigidbody.velocity);
            velocity.y = 0;
            velocity = transform.TransformDirection(velocity);
            Vector3 velocityChange = transform.InverseTransformDirection(targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            velocityChange = transform.TransformDirection(velocityChange);
 
            rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
 
            if (Input.GetButton("Jump"))
            {
                rigidbody.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
            }
        
    }
}
 