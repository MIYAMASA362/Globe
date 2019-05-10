using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatGround : MonoBehaviour
{
    public FloatType.Type type;
    public Transform wireObject;
    private WireFrameTrigger wireFrameTrigger;
    private Renderer wireRenderer;

    public float floatHeight = 8.0f;
    public float floatSpeed = 5.0f;
    public bool isFloat = false;
    public bool onGround = false;
    private float startHeight;
    private float startWireHeight;


    // Use this for initialization
    void Start()
    {
        startHeight = transform.localPosition.y;
        startWireHeight = wireObject.transform.position.magnitude;
        wireObject.gameObject.SetActive(true);
//        wireObject.transform.rotation = transform.rotation;
        wireRenderer = wireObject.GetComponent<Renderer>();
        wireFrameTrigger = wireObject.GetComponent<WireFrameTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFloat && FlagManager.Instance.flagActive &&
            FlagManager.Instance.curFloatType == type)
        {
            onGround = wireFrameTrigger.onTrigger;
            transform.parent.parent = RotationManager.Instance.rotationTransform;
            wireRenderer.enabled = true;
            MoveHeight(startHeight + floatHeight);
        }
        else
        {
            onGround = false;
            transform.parent.parent = RotationManager.Instance.planetTransform;
            wireRenderer.enabled = false;
            MoveHeight(startHeight);
        }

        wireObject.transform.position = wireObject.transform.forward * startWireHeight;
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
