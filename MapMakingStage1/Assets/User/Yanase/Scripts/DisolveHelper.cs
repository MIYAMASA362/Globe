
using UnityEngine;
using SA;

public class DisolveHelper : MonoBehaviour
{
    public StateManager stateManager;
    public Disolve gameCharacter;
    public Disolve goalCharacter;
    public Transform devicePivot;

    private void Start()
    {
        gameCharacter.SetDisolve(0.0f);
        goalCharacter.SetDisolve(1.0f);
    }

    private void Update()
    {

    }

    public void PlayerEnd()
    {
        stateManager.state = StateManager.State.End;
    }

    public void StartFadeUnFade()
    {
        gameCharacter.FadeDsiolve();
        AxisDevice device = stateManager.axisDevice;
        device.chaseTarget = devicePivot;
        device.chaseSpeed = 50.0f;
        device.collider.enabled = false;
        goalCharacter.UnFadeDsiolve();
    }
}
