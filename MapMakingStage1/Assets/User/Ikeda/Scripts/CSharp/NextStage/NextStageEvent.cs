using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class NextStageEvent : MonoBehaviour {

    [SerializeField] private Animator animator;

    [Space(8)]
    [SerializeField] private CinemachineVirtualCamera FollowCamera;
    [SerializeField] private GameObject NowStage;
    [SerializeField] private GameObject NextStage;
    [SerializeField] private GameObject Player;

    private GameObject OldTarget;
    private GameObject Target;

    private float LerpTime = 0f;
    [SerializeField] private float time = 3f;
    

    private bool IsEnable = true;

	// Use this for initialization
	void Start ()
    {
        Target = OldTarget= NowStage;
	}
	
	// Update is called once per frame
	void Update ()
    {
        LerpTime += Time.deltaTime;
        float t = LerpTime/time;
        this.transform.position = Vector3.Lerp(OldTarget.transform.position, Target.transform.position, t);
        if (t >= 1f && IsEnable) End_Transition();

    }

    public void End_Transition()
    {
        animator.SetTrigger("TransitionTrigger");
        
        IsEnable = false;
    }

    public void Next_Transition()
    {
        OldTarget = Target;
        Target = NextStage;
        LerpTime = 0f;
        Player.transform.LookAt(Target.transform);
        IsEnable = true;
    }

    //--- Animation Event -----------------------------------------------------

    public void Follow_Player()
    {
        FollowCamera.Follow = Player.transform;
    }

    public void Follow_NowStage()
    {
        FollowCamera.Follow = NowStage.transform;
    }

    public void Follow_NextStage()
    {
        FollowCamera.Follow = NextStage.transform;
    }
}
