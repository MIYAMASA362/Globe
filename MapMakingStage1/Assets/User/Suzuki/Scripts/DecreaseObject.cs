using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DecreaseObject : MonoBehaviour {

    //オブジェクトを消す（作用）させることができるギミックのタグ
    [Tag] public string Tag;

    [SerializeField] float value = 0.0f;
    [SerializeField] float reducedSpeed = 0.0f;

    float InitValue = 0.0f;
    Vector3 InitScale = Vector3.zero;

    // Use this for initialization
    void Start () {

        InitValue = value;
        InitScale = this.gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update () {

        this.gameObject.transform.localScale = InitScale * Mathf.Min(value / InitValue, 1f);
        if (value <= 0f)
        {
            this.gameObject.SetActive(false);
        }

    }


    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == Tag)
        {

            value -= reducedSpeed;
        }
    }

}
