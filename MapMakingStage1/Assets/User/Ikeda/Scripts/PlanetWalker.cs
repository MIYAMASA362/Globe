using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlanetWalker : MonoBehaviour {

    //--- Attribute -----------------------------

    //--- public ----------------------

    //--- private ---------------------
    [Header("Stats")]
    [SerializeField] Transform cameraTransform;
    [SerializeField] LayerMask Hitlayer;
    [Space(10)]
    [SerializeField] float speed = 2f;
    [SerializeField] float runSpeed = 7f;
    [SerializeField] float castDistance = 0.5f;
    [SerializeField] float maxVelocityChange = 0.5f;
    [SerializeField] float rayLength = 0.75f;
    [SerializeField] float castHeight = 0.5f;

    [Header("Status")]
    [SerializeField] bool onGround = false;
    [SerializeField] float normalvsAngle = 0f;

    //--- private ---------------------
    new Rigidbody rigidbody = null;
    RaycastHit casthit;

    //Input
    bool jump;
    float horizontal;
    float vertical;

    //Move
    Vector3 MoveVec;
    [SerializeField]Vector3 oldPosition;

    //--- MonoBehavior --------------------------

	// Use this for initialization
	void Start ()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        oldPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Get_Input();
    }

    void FixedUpdate()
    {
        Get_RayCast();
        MoveVec = MoveDirection();
        Move(MoveVec);
    }

    void LateUpdate()
    {
        
    }

    //--- Method --------------------------------

    //Input
    void Get_Input()
    {
        jump        = Input.GetButton(InputManager.Jump);
        horizontal  = Input.GetAxis(InputManager.Horizontal);
        vertical    = Input.GetAxis(InputManager.Vertical);
    }

    //RayCast
    void Get_RayCast()
    {
        Vector3 origin = this.transform.position + (rigidbody.velocity.normalized * castDistance) + (this.transform.up * castHeight);

        Debug.DrawRay(origin, -this.transform.up * rayLength, Color.red);
        if (Physics.Raycast(origin, -this.transform.up * rayLength, out casthit, rayLength, Hitlayer))
            onGround = true;
        else
            onGround = false;
    }

    //MoveDirection
    Vector3 MoveDirection()
    {
        Vector3 forward = Vector3.Cross(this.transform.up,-cameraTransform.right).normalized;
        Vector3 right = Vector3.Cross(this.transform.up,forward).normalized;
        return (forward * vertical + right * horizontal);
    }

    //Charactor Move
    void Move(Vector3 MoveDir)
    {
        rigidbody.AddForce(VelocityChanger(MoveDir * speed), ForceMode.VelocityChange);

        if (!onGround)
            this.transform.position = oldPosition;
        else
            oldPosition = this.transform.position;

        return;
    }

    //VelocityChanger 未使用
    Vector3 VelocityChanger(Vector3 targetVelocity)
    {
        Vector3 velocity = transform.InverseTransformDirection(rigidbody.velocity);
        velocity.y = 0f;
        velocity = transform.TransformDirection(velocity);

        Vector3 velocityChange = transform.InverseTransformDirection(targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        velocityChange = transform.TransformDirection(velocityChange);

        return velocityChange;
    }
}
