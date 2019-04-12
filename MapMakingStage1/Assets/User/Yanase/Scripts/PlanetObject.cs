using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SA;

public class PlanetObject : MonoBehaviour
{
    public Vector3 moveDir = Vector3.zero;
    public float maxVelocityChange = 10.0f;
    [SerializeField] LayerMask Hitlayer;
    [SerializeField] private float rayDistance = 0.5f;
    [SerializeField] private float rayBigin = 0.2f;
    [SerializeField] private float rayEnd = -0.2f;


    private Rigidbody rigidbody = null;

    public bool onGround;

    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.interpolation = RigidbodyInterpolation.Extrapolate;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotationY;
        rigidbody.useGravity = false;
        rigidbody.drag = 3;
        rigidbody.angularDrag = 999;
    }

    void Update()
    {
       
    }

    void LateUpdate()
    {
        // 移動方向を取得
        moveDir = RotationManager.Instance.GetMoveDir(this.transform.position);

        onGround = OnGround();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moveDir != Vector3.zero && !onGround)
        {
            Vector3 targetVelocity = moveDir * 2.0f;

            Vector3 velocity = transform.InverseTransformDirection(rigidbody.velocity);
            velocity.y = 0;
            velocity = transform.TransformDirection(velocity);
            Vector3 velocityChange = transform.InverseTransformDirection(targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            velocityChange = transform.TransformDirection(velocityChange);
            rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
        }

        // 重力
        Transform planet = RotationManager.Instance.planetTransform;
        rigidbody.AddForce((planet.position - transform.position) * 3.0f);
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    //RayCast
    bool OnGround()
    {
        bool hit = false;

        Vector3 planetPosition = RotationManager.Instance.planetTransform.position;
        RaycastHit castHit;
        float rayLength = (rayBigin - rayEnd);
        Vector3 origin = this.transform.position + (moveDir.normalized * rayDistance) + (transform.up * rayBigin);
        Vector3 rayDir = -transform.up * rayLength;
        
        Debug.DrawRay(origin, rayDir, Color.red);
        if (Physics.Raycast(origin, rayDir, out castHit, rayLength, Hitlayer))
        {
            if(castHit.transform.gameObject != this.transform.gameObject) hit = true;
        }

        return hit;
    }
}
