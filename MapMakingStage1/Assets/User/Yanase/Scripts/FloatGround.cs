using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatGround : MonoBehaviour
{
    public Transform wireObject;
    private Renderer wireRenderer;

    public float floatHeight = 8.0f;
    public float floatSpeed = 5.0f;
    public bool isFloat = false;
    public LayerMask hitLayer = LayerMask.NameToLayer("Ground") | LayerMask.NameToLayer("Character");
    private float startHeight;
    public bool onGround = false;
    bool onFloat = false;

    public float rayRadius = 0.75f;
    [Header("OnFloat == false")]
    public float rayStart1 = 0.2f;
    public float rayLength1 = 0.3f;
    [Header("OnFloat == true")]
    public float rayStart2 = 0.2f;
    public float rayLength2 = 0.3f;

    Vector3 origin;
    Vector3 end;


    // Use this for initialization
    void Start()
    {
        startHeight = transform.localPosition.y;
        wireObject.gameObject.SetActive(true);
        wireObject.transform.rotation = transform.rotation;
        wireRenderer = wireObject.GetComponent<Renderer>();
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
            wireRenderer.enabled = true;
            MoveHeight(floatHeight);
        }
        else
        {
            transform.parent.parent = RotationManager.Instance.planetTransform;
            wireRenderer.enabled = false;
            MoveHeight(startHeight);
        }

        wireObject.transform.localPosition = new Vector3(0.0f, subTargetHeight, 0.0f);
        
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
        float rayLength = 0.0f;

        if (isFloat)
        {
            rayLength = rayLength2;
            origin = transform.position + (dir * rayStart2);
        }
        else
        {
            rayLength = rayLength1;
            origin = transform.position + (dir * rayStart1);
        }
        end = origin + (dir * (rayLength));

        RaycastHit castHit;

        if (Physics.SphereCast(origin, rayRadius, dir.normalized, out castHit, rayLength, hitLayer))
        {
            onGround = true;
            end = castHit.point;
        }

        if (onGround) wireRenderer.material.SetColor("_Color", Color.red);
        else wireRenderer.material.SetColor("_Color", Color.green);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(origin, rayRadius);
        Gizmos.DrawLine(origin, end);
        Gizmos.DrawWireSphere(end, rayRadius);
    }
}
