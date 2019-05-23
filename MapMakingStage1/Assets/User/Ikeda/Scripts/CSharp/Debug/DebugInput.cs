using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInput : MonoBehaviour {

    [Header("State")]
    public float Mouse_X;
    public float Mouse_Y;
    public float Mouse_ScrollWheel;
    public bool Fire1;
    public bool Fire2;
    public bool Fire3;
    public bool Jump;
    public bool Submit;
    public bool Cancel;

    //XBOX
    [Space(4)]
    public float Horizontal;
    public float Vertical;
    public bool LeftPush;
    public bool LeftBack;
    public float LeftTrigger;

    [Space(4)]
    public float Right_Horizontal;
    public float Right_Vertical;
    public bool RightPush;
    public bool RightBack;
    public float RightTrigger;

    [Space(4)]
    public bool View;
    public bool Menu;
    public float X_Arrow;
    public float Y_Arrow;
    public bool A_Button;
    public bool B_Button;
    public bool X_Button;
    public bool Y_Button;

    [Space(4)]
    public bool Up;
    public bool Down;
    public bool Left;
    public bool Right;

    [Space(4)]
    public bool On;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        
        Mouse_X = Input.GetAxis(InputManager.Mouse_X);
        Mouse_Y = Input.GetAxis(InputManager.Mouse_Y);
        Mouse_ScrollWheel = Input.GetAxis(InputManager.Mouse_ScrollWheel);
        Fire1 = Input.GetButton(InputManager.Fire1);
        Fire2 = Input.GetButton(InputManager.Fire2);
        Fire3 = Input.GetButton(InputManager.Fire3);
        Jump = Input.GetButton(InputManager.Jump);
        Submit = Input.GetButton(InputManager.Submit);

        Horizontal = Input.GetAxis(InputManager.Horizontal);
        Vertical = Input.GetAxis(InputManager.Vertical);
        LeftPush = Input.GetButton(InputManager.Left_Push);
        LeftBack = Input.GetButton(InputManager.Left_Back);
        LeftTrigger = Input.GetAxis(InputManager.Left_AxisRotation);

        Right_Horizontal = Input.GetAxis(InputManager.Camera_Horizontal);
        Right_Vertical = Input.GetAxis(InputManager.Camera_Vertical);
        RightPush = Input.GetButton(InputManager.View_Swith);
        RightBack = Input.GetButton(InputManager.Right_Back);
        RightTrigger = Input.GetAxis(InputManager.Right_AxisRotation);

        View = Input.GetButton(InputManager.View);
        Menu = Input.GetButton(InputManager.Menu);
        X_Arrow = Input.GetAxis(InputManager.X_Selecter);
        Y_Arrow = Input.GetAxis(InputManager.Y_Selecter);
        A_Button = Input.GetButton(InputManager.Set_EarthAxis);
        B_Button = Input.GetButton(InputManager.Change_AscDes);
        X_Button = Input.GetButton(InputManager.X_Button);
        Y_Button = Input.GetButton(InputManager.Y_Button);

        Up = Input.GetButton(InputManager.Cross_Up);
        Down = Input.GetButton(InputManager.Cross_Down);
        Right = Input.GetButton(InputManager.Cross_Right);
        Left = Input.GetButton(InputManager.Cross_Left);

        On = Input.GetButton(InputManager.Y_Selecter);
    }
}
