using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : Singleton<RotationManager> {

    [SerializeField,Header("星")]
    private Transform Planet = null;

    [SerializeField, Header("回転させる")]
    private Transform RotTarget = null;

    [SerializeField,Header("旗")]
    private Transform Flag = null;

    [SerializeField,Header("地軸(回転軸)")]
    private Vector3 EarthAxis = Vector3.zero;

    [SerializeField]
    private GameObject AxisObj = null;

    [SerializeField, Header("回転方向")]
    private Vector3 AxisDir = Vector3.zero;

    public Quaternion q;

    [SerializeField, Header("回転速度")]
    private float Angle;

    //Initialize
	private void Start ()
    {
        AxisDir = Vector3.zero;

        q = Quaternion.AngleAxis(Mathf.PI / 180, EarthAxis).normalized;
        AxisDir = q.eulerAngles;
        if(AxisObj == null) AxisObj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
    }
	
    //Update
	private void Update ()
    {
        Set_EarthAxis(Flag.transform.position);
        DebugMethod();

        AxisObj.transform.position = Planet.position;
        AxisObj.transform.rotation = Quaternion.FromToRotation(Vector3.up,EarthAxis);
    }

    //FixedUpdate
    private void FixedUpdate()
    {
        
    }

    private void DebugMethod()
    {
        if (RotTarget == null) return;
        //Debug用に組んでいるのでUpdate書き変えてもいいよ

        //回転軸の設定
        Debug.DrawRay(Planet.position, EarthAxis, Color.red);   //地軸のDebug表示
        RotTarget.transform.rotation = Quaternion.AngleAxis(Angle * Time.deltaTime, EarthAxis.normalized) * RotTarget.transform.rotation;  //回転オブジェクトを回転させる

        q = Quaternion.AngleAxis(Mathf.PI / 180, EarthAxis).normalized;
        AxisDir = q.eulerAngles;
    }

    //回転軸の設定
    public void Set_EarthAxis(Vector3 FlagPos)
    {
        //距離ベクトル
        EarthAxis = FlagPos - Planet.position;
        //方向ベクトル(地軸)
        EarthAxis = EarthAxis.normalized;
    }

    //地軸の取得
    public Vector3 Get_EarthAxis()
    {
        return this.EarthAxis;
    }

    //中心位置の設定
    public void Set_CorePosition(Transform Planet)
    {
        this.Planet = Planet;
    }
}
