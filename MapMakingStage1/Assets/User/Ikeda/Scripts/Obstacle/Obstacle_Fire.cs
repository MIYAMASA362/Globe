using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Fire : MonoBehaviour
{

    [SerializeField] float value = 0f;
    [SerializeField] Transform ScaleDown;

    float InitValue = 0f;
    Vector3 InitScale = Vector3.zero;

	// Use this for initialization
	void Start ()
    {
        InitValue = value;
        InitScale = ScaleDown.localScale;
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.ScaleDown.localScale = InitScale * Mathf.Min(value / InitValue,1f);

	    if(value <= 0f)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("name:" + other.transform.name);

        if (other.transform.tag == "Rain")
        {
            Debug.Log("Stay!!!");

            value -= 0.1f;
        }
    }

}
