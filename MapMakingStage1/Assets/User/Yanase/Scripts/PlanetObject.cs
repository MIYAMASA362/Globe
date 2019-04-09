using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SA;

public class PlanetObject : MonoBehaviour
{
    public Vector3 moveDir = Vector3.zero;
    public float speed = 1.0f;
    public float maxVelocityChange = 10.0f;

    private float xRotation;
    private float yRotation;
    private Rigidbody rigidbody = null;

    public Vector3 velocity;

    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        // 移動方向を取得
        moveDir = RotationManager.Instance.GetMoveDir(this.transform.position);

        velocity = rigidbody.velocity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moveDir != Vector3.zero)
        {
            Vector3 targetVelocity = moveDir * speed;

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

        Transform planet = RotationManager.Instance.planetTransform;
        rigidbody.AddForce((planet.position - transform.position) * 3.0f);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            Debug.Log("Stay!!!");

            Rigidbody otherRigid = other.GetComponent<Rigidbody>();
            if (otherRigid == null) return;

            Vector3 vec = this.rigidbody.velocity.normalized;
            vec.y = 0f;

            //otherRigid.AddForce(,ForceMode.VelocityChange);
        }
    }
}
