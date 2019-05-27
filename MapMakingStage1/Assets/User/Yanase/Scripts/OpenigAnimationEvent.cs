using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;

public class OpenigAnimationEvent : MonoBehaviour {

    private GameObject startCharacter;
    private StateManager gameCharacter;
    

    public void Init(GameObject startCharacter, StateManager gameCharacter)
    {
        this.startCharacter = startCharacter;
        this.gameCharacter = gameCharacter;
        gameCharacter.state = StateManager.State.Start;
    }

	public void ChangeCharacter()
    {
        startCharacter.SetActive(false);
        gameCharacter.gameObject.SetActive(true);

        gameCharacter.transform.position = startCharacter.transform.position;
        gameCharacter.state = StateManager.State.Start;
        gameCharacter.anim.SetTrigger("falling");
    }

    public void EndAnimation()
    {
        GameObject.Destroy(this.gameObject);
    }
}
