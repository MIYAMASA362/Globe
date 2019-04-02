using System.Collections;
using UnityEngine;

namespace  SA
{

public class InputHandler : MonoBehaviour {


	public float vertical;
	public float horizontal;
	public Vector3 moveDir;

	StateManager states;

	public CameraManager camManager;
	public float delta;
	public GameObject pi;

	public Transform CameraPivot;

	//------------------------------------------
	//------------------------------------------
	void Start()
	{

		states = GetComponent<StateManager>();
		states.Init();

		//camManager = //Camera.main.transform.gameObject.GetComponent<CameraManager>();
		camManager.Init(CameraPivot.transform);
		

	}//Start end 
	//------------------------------------------
	void FixedUpdate()
	{
		delta = Time.fixedDeltaTime;
		GetInput();
		UpdateStates();
		camManager.Tick(delta);

	}//Update end
	//------------------------------------------
	void Update()
	{
		delta = Time.deltaTime;
		states.Tick(delta);
	}
	//------------------------------------------
	void GetInput()
	{
		 vertical = Input.GetAxis("Vertical");
		 horizontal = Input.GetAxis("Horizontal");
	}
	//------------------------------------------
	void  UpdateStates()
	{
		states.vertical = vertical;
		states.horizontal = horizontal;

		Vector3 v = states.vertical * pi.transform.forward;
		Vector3 h =  horizontal * pi.transform.right;
		states.moveDir = (v+h).normalized;
		float m = Mathf.Abs(horizontal) +  Mathf.Abs(vertical);
		states.moveAmount = Mathf.Clamp01(m);


		states.FixedTick(Time.deltaTime);
	}


  }

}