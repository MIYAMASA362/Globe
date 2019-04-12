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
    [SerializeField] float maxVelocityChange = 0.5f;
    [SerializeField] float castDistance = 0.15f;
    [SerializeField] float rayStartPosition = 0.4f;
    [SerializeField] float rayEndPosition = -0.5f;
    [SerializeField] float rayRadius = 0.025f;

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

    Vector3 origin;
    Vector3 end;

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
        MoveVec = MoveDirection();
        Get_RayCast();
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
        onGround = false;
        float maxAngle = 40.0f;

        Vector3 velocity = transform.InverseTransformDirection(rigidbody.velocity);
        velocity.y = 0.0f;
        velocity = transform.TransformDirection(velocity);

        Vector3 movePos = this.transform.position + (velocity.normalized * castDistance);
        Vector3 rayPos = this.transform.position + (velocity.normalized * (castDistance * 0.5f)) + (this.transform.up * rayStartPosition);
        origin = movePos + (this.transform.up * rayStartPosition);
        end = movePos + (this.transform.up * rayEndPosition);
        float rayLength = rayStartPosition - rayEndPosition;

        Debug.DrawRay(rayPos, -transform.up * rayLength, Color.red);

        if (Physics.SphereCast(origin, rayRadius, -this.transform.up * rayLength, out casthit, rayLength, Hitlayer))
        {
            end = casthit.point;
            float sphereNormalAngle = Mathf.Acos(Vector3.Dot(transform.up, casthit.normal)) * Mathf.Rad2Deg;
            if (sphereNormalAngle > maxAngle)
            {
                if (Physics.Raycast(rayPos, -this.transform.up * rayLength * 1.5f, out casthit, rayLength * 1.5f, Hitlayer))
                {
                    float rayNormalAngle = Mathf.Acos(Vector3.Dot(transform.up, casthit.normal)) * Mathf.Rad2Deg;
                    // 当たった法線が一定以上なら進めない
                    if (rayNormalAngle > maxAngle)
                    {
                        Vector3 right = transform.position + Vector3.Cross(transform.up, velocity.normalized) * 0.3f + (this.transform.up * rayStartPosition);
                        Vector3 left = transform.position - Vector3.Cross(transform.up, velocity.normalized) * 0.3f + (this.transform.up * rayStartPosition);
                        Debug.DrawRay(right, -transform.up * rayLength, Color.red);
                        Debug.DrawRay(left, -transform.up * rayLength, Color.red);
                        if (!(Physics.Raycast(right, -this.transform.up * rayLength * 1.5f, out casthit, rayLength * 1.5f, Hitlayer) &&
                              Physics.Raycast(left, -this.transform.up * rayLength * 1.5f, out casthit, rayLength * 1.5f, Hitlayer)))
                        {
                            return;
                        }
                    }
                }
            }
            onGround = true;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(origin, rayRadius);
        Gizmos.DrawWireSphere(end, rayRadius);
        Gizmos.DrawLine(origin, end);
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

    //VelocityChanger
    public Vector3 VelocityChanger(Vector3 targetVelocity)
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
