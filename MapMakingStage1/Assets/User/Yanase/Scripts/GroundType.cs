using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundType : MonoBehaviour {

    public enum Type
    {
        Grass,
        Rock,
        Snow,
        Sand
    };

    public Type type;
}
