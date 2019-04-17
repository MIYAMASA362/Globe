using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSwitch : ObjectController
{
    void OnTriggerEnter(Collider hit)
    {
        // 接触対象はPlayerタグですか？
        if (hit.CompareTag("Player"))
        {
            OnBotton();
        }
    }

}
