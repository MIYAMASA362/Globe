using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatGround : MonoBehaviour
{
//     public Transform subObject;

    public float floatHeight = 10.0f;
    public float floatSpeed = 1.0f;
    public bool isFloat = false;
    public LayerMask hitLayer = LayerMask.NameToLayer("Ground");
    private float startHeight;
    public bool onGround = false;
    bool onFloat = false;

    public float rayRadius = 0.7f;
    public float rayStart = 0.2f;
    public float rayLength = 0.3f;

    Vector3 origin;
    Vector3 end;


    // Use this for initialization
    void Start()
    {
        startHeight = transform.localPosition.y;
//      subObject.transform.rotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (onFloat != FlagManager.Instance.onFloat)
        {
            isFloat = !isFloat;
            onFloat = FlagManager.Instance.onFloat;
        }

        float subTargetHeight = 0.0f;

        if (isFloat)
        {
            transform.parent.parent = RotationManager.Instance.rotationTransform;
            subTargetHeight = startHeight;
            MoveHeight(floatHeight);
        }
        else
        {
            transform.parent.parent = RotationManager.Instance.planetTransform;
            subTargetHeight = floatHeight;
            MoveHeight(startHeight);
        }

 //       subObject.transform.localPosition = new Vector3(0.0f, subTargetHeight, 0.0f);
        
    }

    private void FixedUpdate()
    {
        OnSphereCast();
    }

    void MoveHeight(float target)
    {
        if (transform.localPosition.y != target)
        {
            float height = Mathf.Lerp(transform.localPosition.y, target, floatSpeed * Time.deltaTime);
            transform.localPosition = new Vector3(0.0f, height, 0.0f);
        }
    }

    void OnSphereCast()
    {
        onGround = false;
        Vector3 centerPos = RotationManager.Instance.planetTransform.position;
        Vector3 dir = centerPos - transform.position;
        float len = 0.0f;

        if (!isFloat)
        {
            dir *= -1f;
            len = 0.4f;
        }

        origin = transform.position + (dir * rayStart);
        end = origin + (dir * (rayLength + len));

        RaycastHit castHit;

        if (Physics.SphereCast(origin, rayRadius, dir.normalized, out castHit, rayLength + len, hitLayer))
        {
            onGround = true;
            end = castHit.point;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(origin, rayRadius);
        Gizmos.DrawLine(origin, end);
        Gizmos.DrawWireSphere(end, rayRadius);
    }
}
