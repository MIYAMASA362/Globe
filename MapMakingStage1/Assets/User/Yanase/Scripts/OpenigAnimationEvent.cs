using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;

public class OpenigAnimationEvent : MonoBehaviour {

    public Animator anim;
    private GameObject startCharacter;
    private StateManager gameCharacter;
    

    public void Init(GameObject startCharacter, StateManager gameCharacter)
    {
        this.startCharacter = startCharacter;
        this.gameCharacter = gameCharacter;
        gameCharacter.state = StateManager.State.Start;
        anim.enabled = false;
    }

    public void AnimationStart()
    {
        anim.enabled = true;
    }

    public void ChangeCharacter()
    {
        startCharacter.SetActive(false);
        gameCharacter.gameObject.SetActive(true);
        gameCharacter.axisDevice.gameObject.SetActive(true);

        Vector3 dir = (RotationManager.Instance.planetTransform.position - gameCharacter.transform.position).normalized;
        gameCharacter.transform.position = startCharacter.transform.position + dir * 0.8f;
        gameCharacter.transform.rotation = startCharacter.transform.rotation;
        gameCharacter.axisDevice.transform.position = startCharacter.transform.position;

        gameCharacter.state = StateManager.State.Start;
        gameCharacter.anim.SetTrigger("falling");
    }

    public void EndAnimation()
    {
        GameObject.Destroy(this.gameObject);
    }
}
