using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatGround : MonoBehaviour
{
    public Transform wireObject;
    private WireFrameTrigger wireFrameTrigger;
    private Renderer wireRenderer;

    public float floatHeight = 8.0f;
    public float floatSpeed = 5.0f;
    public bool isFloat = false;
    public bool onGround = false;
    private bool onFloat = false;
    private float startHeight;
    private float startWireHeight;


    // Use this for initialization
    void Start()
    {
        startHeight = transform.localPosition.y;
        startWireHeight = wireObject.transform.localPosition.y - startHeight;
        wireObject.gameObject.SetActive(true);
        wireObject.transform.rotation = transform.rotation;
        wireRenderer = wireObject.GetComponent<Renderer>();
        wireFrameTrigger = wireObject.GetComponent<WireFrameTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        onGround = wireFrameTrigger.onTrigger;

        if (onFloat != FlagManager.Instance.onFloat)
        {
            isFloat = !isFloat;
            onFloat = FlagManager.Instance.onFloat;
        }

        float subTargetHeight = 0.0f;

        if (isFloat)
        {
            transform.parent.parent = RotationManager.Instance.rotationTransform;
            subTargetHeight = startHeight + startWireHeight;
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

    void MoveHeight(float target)
    {
        if (transform.localPosition.y != target)
        {
            float height = Mathf.Lerp(transform.localPosition.y, target, floatSpeed * Time.deltaTime);
            transform.localPosition = new Vector3(0.0f, height, 0.0f);
        }
    }
}
