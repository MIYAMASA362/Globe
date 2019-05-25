using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireFrameTrigger : MonoBehaviour
{
    public bool onTrigger = false;
    public LayerMask hitLayer;
    public Transform myCollider;
    private List<GameObject> triggerList = new List<GameObject>();
    Material matrial;

    // Use this for initialization
    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        matrial = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        onTrigger = (triggerList.Count > 0) ? true : false;

        if (onTrigger)
        {
            matrial.SetColor("_Color", Color.red);
            matrial.SetColor("_EmissionColor", new Color(1, 0, 0, 1));
        }
        else
        {
            matrial.SetColor("_Color", Color.green);
            matrial.SetColor("_EmissionColor", new Color(0, 1, 0, 1));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CompareLayer(hitLayer,other.gameObject.layer)) 
        {
            if (myCollider == other.transform)
            {
                
                return;
            }
            if (triggerList.Contains(other.gameObject)) return;

            triggerList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (CompareLayer(hitLayer, other.gameObject.layer))
        {
            if (myCollider == other.transform)
            {
                return;
            }

            triggerList.Remove(other.gameObject);
        }
    }

    // LayerMaskに対象のLayerが含まれているかチェックする
    private bool CompareLayer(LayerMask layerMask, int layer)
    {
        return ((1 << layer) & layerMask) != 0;
    }
}
