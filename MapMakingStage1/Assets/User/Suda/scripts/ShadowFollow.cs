using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowFollow : MonoBehaviour {

    //Sunの変数
    public GameObject LookPlanet;

    // initialization
    void Start()
    {

    }

    // Update
    void Update()
    {
        //Sunの向き更新
        transform.LookAt(LookPlanet.transform);
    }
}
