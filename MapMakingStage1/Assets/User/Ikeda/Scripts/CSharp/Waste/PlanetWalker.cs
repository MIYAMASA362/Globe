using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;

[RequireComponent(typeof(Rigidbody))]
public class PlanetWalker : MonoBehaviour {

    //--- Attribute -----------------------------


    //--- private ---------------------
    [Header("Stats")]
    [SerializeField] LayerMask Hitlayer;
    [Space(10)]
    [SerializeField] float speed = 2f;
    [SerializeField] float maxVelocityChange = 0.5f;
    [SerializeField] float castDistance = 0.15f;
    [SerializeField] float rayStartPosition = 0.3f;
    [SerializeField] float rayEndPosition = -0.5f;
    [SerializeField] float rayRadius = 0.013f;
    [SerializeField] float stopMaxAngle = 70.0f;
    

    [Header("Status")]
    [SerializeField] bool onGround = false;

    //--- private ---------------------
    public new Rigidbody rigidbody = null;
    RaycastHit casthit;

    //Input

    //Move
    Vector3 MoveVec;
    public float moveAmount;
    public float turnSpeed;
    [SerializeField] public Vector3 oldPosition;
    [SerializeField] public Vector3 defaultScale;

    private Vector3 origin;
    private Vector3 end;

    public Animator anim;

    public float horizontal;
    public float vertical;


    // Use this for initialization
    void Start ()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        defaultScale = this.transform.lossyScale;
        anim = GetComponent<StateManager>().anim;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 lossScale = transform.lossyScale;
        Vector3 localScale = transform.localScale;
        transform.localScale = new Vector3(
                localScale.x / lossScale.x * defaultScale.x,
                localScale.y / lossScale.y * defaultScale.y,
                localScale.z / lossScale.z * defaultScale.z
        );
    }

    //RayCast
    void Get_RayCast()
    {
        onGround = false;

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
            float sphereNormalAngle = Mathf.Acos(Vector3.Dot(transform.up, casthit.normal)) * Mathf.Rad2Deg;
            if (sphereNormalAngle > stopMaxAngle)
            {
                if (Physics.Raycast(rayPos, -this.transform.up * rayLength, out casthit, rayLength, Hitlayer))
                {
                    float rayNormalAngle = Mathf.Acos(Vector3.Dot(transform.up, casthit.normal)) * Mathf.Rad2Deg;
                    // 当たった法線が一定以上なら進めない
                    if (rayNormalAngle > stopMaxAngle)
                    {
                        Vector3 right = transform.position + Vector3.Cross(transform.up, velocity.normalized) * 0.2f + (this.transform.up * rayStartPosition);
                        Vector3 left = transform.position - Vector3.Cross(transform.up, velocity.normalized) * 0.2f + (this.transform.up * rayStartPosition);
                        Debug.DrawRay(right, -transform.up * rayLength, Color.red);
                        Debug.DrawRay(left, -transform.up * rayLength, Color.red);
                        if (!(Physics.Raycast(right, -this.transform.up * rayLength, out casthit, rayLength * 1.5f, Hitlayer) &&
                              Physics.Raycast(left, -this.transform.up * rayLength, out casthit, rayLength * 1.5f, Hitlayer)))
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
        Transform cameraTransform = Camera.main.transform;
        Vector3 forward = Vector3.Cross(this.transform.up,-cameraTransform.right).normalized;
        Vector3 right = Vector3.Cross(this.transform.up,forward).normalized;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        return (forward * vertical + right * horizontal).normalized;
    }

    //Charactor Move
    void Move(Vector3 MoveDir)
    {
        Vector3 velocityChanger = VelocityChanger(MoveDir * speed * moveAmount * Time.deltaTime);
        Transform gravityCenter = RotationManager.Instance.planetTransform;

        Quaternion q = Quaternion.FromToRotation(transform.forward, MoveDir) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * moveAmount * turnSpeed);

        rigidbody.AddForce(velocityChanger, ForceMode.VelocityChange);

        if (transform.parent)
        {
            if (!onGround)
                this.transform.localPosition = oldPosition;
            else
                oldPosition = this.transform.localPosition;
        }
        else
        {
            if (!onGround)
                this.transform.position = oldPosition;
            else
                oldPosition = this.transform.position;
        }
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

    public void FixedTick(float d)
    {
        MoveVec = MoveDirection();
        Get_RayCast();
        Move(MoveVec);

        if (anim) anim.SetFloat("move", moveAmount);
    }
}
