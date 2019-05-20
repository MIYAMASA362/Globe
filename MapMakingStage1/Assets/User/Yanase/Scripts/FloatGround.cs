using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatGround : MonoBehaviour
{
    public FloatType.Type type;
    public WireFrameTrigger wireFrameTrigger;
    public GameObject effectObject;
    public Animator animator;

    private Renderer effectRender;

    public float floatHeight = 8.0f;
    public float floatSpeed = 5.0f;
    public bool isFloat = false;
    public bool onGround = false;
    private float startHeight;
    private float startWireHeight;

    public bool isFalse = false;

    void Start()
    {
        startHeight = transform.localPosition.y;
        startWireHeight = wireFrameTrigger.transform.position.magnitude;
        wireFrameTrigger.gameObject.SetActive(false);
        effectRender = effectObject.GetComponent<Renderer>();

        effectObject.SetActive(false);
        animator.enabled = true;
    }

    void Update()
    {
        animator.SetBool("false", isFalse);
        if (isFalse) isFalse = false;

        if (FlagManager.Instance.flagActive &&
            FlagManager.Instance.curFloatType == type)
        {
            effectObject.SetActive(true);

            if (isFloat)
            {
                onGround = wireFrameTrigger.onTrigger;

                SetUpdate(RotationManager.Instance.rotationTransform, startHeight + floatHeight, true);

                animator.SetBool("move", true);

                if (wireFrameTrigger.onTrigger)
                {
                    effectRender.material.SetColor("_ShieldPatternColor", Color.red);
                }
                else
                {
                    effectRender.material.SetColor("_ShieldPatternColor", Color.green);
                }
            }
            else
            {
                onGround = false;
                effectRender.material.SetColor("_ShieldPatternColor", Color.cyan);
                animator.SetBool("move", false);

                SetUpdate(RotationManager.Instance.planetTransform, startHeight, false);
            }
        }
        else
        {
            onGround = false;
            effectObject.SetActive(false);
            animator.SetBool("move", false);

            SetUpdate(RotationManager.Instance.planetTransform, startHeight, false);

        }

        wireFrameTrigger.transform.position = wireFrameTrigger.transform.forward * startWireHeight;
    }

    void SetUpdate(Transform parent, float height, bool wireActive)
    {
        transform.parent.parent = parent;
        MoveHeight(height);
        wireFrameTrigger.gameObject.SetActive(wireActive);
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
