using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlanetWalker : MonoBehaviour {

    //--- Attribute -----------------------------

    //--- public ----------------------
    [Header("Stats")]
    public Transform cameraTransform;
    public Transform raycastTransform;
    public LayerMask Hitlayer;
    [Space(10)]
    public float speed = 2f;
    public float runSpeed = 7f;

    [Header("Status")]
    public bool onGround = false;

    //--- private ---------------------
    private Rigidbody rigidbody = null;
    private RaycastHit casthit;

    //Input
    private bool jump;
    private float horizontal;
    private float vertical;

    //MoveChanger
    private Vector3 oldPosition;

    //--- MonoBehavior --------------------------

	// Use this for initialization
	void Start ()
    {
        rigidbody = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        oldPosition = this.transform.position;

        Get_Input();
        Get_RayCast();
	}

    void FixedUpdate()
    {
        
    }

    void LateUpdate()
    {

    }

    //--- Method --------------------------------

    //Input
    void Get_Input()
    {
        jump        = Input.GetButton(InputManager.Jump);
        horizontal  = Input.GetAxis(InputManager.Horizontal);
        vertical    = Input.GetAxis(InputManager.Vertical);
    }

    //RayCast
    void Get_RayCast()
    {
        Debug.DrawRay(this.transform.position, -this.transform.up * 10f, Color.red);
        if (Physics.Raycast(this.transform.position, -this.transform.up, out casthit, float.MaxValue, Hitlayer))
        {
            onGround = true;
        }
        else onGround = false;
    }

    //
}
