using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBase : MonoBehaviour
{
    public enum State
    {
        Idle,
        Get,
        Follow
    }

    protected State state;

    [Header("Idle")]
    public float RotSpeed = 3f;

    // Use this for initialization
    void Start ()
    {
        state = State.Idle;
	}
	
	// Update is called once per frame
	public virtual void Update ()
    {
        this.transform.rotation *= Quaternion.AngleAxis(RotSpeed * Time.deltaTime * 80f, Vector3.up);
    }

}
