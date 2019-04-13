using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DecreaseObject : MonoBehaviour {

    //オブジェクトを消す（作用）させることができるギミックのタグ
    [Tag] public string Tag;

    [SerializeField] float value = 0f;
  //  [SerializeField] Transform ScaleDown;

    float InitValue = 0f;
    Vector3 InitScale = Vector3.zero;

    // Use this for initialization
    void Start () {

        InitValue = value;
        InitScale = this.gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update () {

       // this.ScaleDown.localScale = InitScale * Mathf.Min(value / InitValue, 1f);
        this.gameObject.transform.localScale = InitScale * Mathf.Min(value / InitValue, 1f);
        if (value <= 0f)
        {
            this.gameObject.SetActive(false);
        }

    }


    private void OnTriggerStay(Collider other)
    {
        Debug.Log("name:" + other.transform.name);

        if (other.transform.tag == Tag)
        {
            Debug.Log("Stay!!!");

            value -= 0.1f;
        }
    }

}
