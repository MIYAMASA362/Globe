using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;

public class FootStepEvent : MonoBehaviour {

    public StateManager stateManager = null;
    public PlanetWalker planetWalker = null;
    AudioSource footAudio;

    public float grass_Volume = 0.5f;
    public float snow1_Volume = 2.5f;
    public float snow2_Volume = 1.8f;
    public float rock_Volume = 1.5f;
    public float sand_Volume = 1.5f;

    private void Start()
    {
        footAudio = GetComponent<AudioSource>();
        if (!footAudio) footAudio = gameObject.AddComponent<AudioSource>();
    }

    void FootStep1()
    {
        if (!stateManager.onGround) return;
        if (planetWalker.moveAmount < 0.1f) return;
        AudioManager audioManager = AudioManager.Instance;

        switch(stateManager.groundType)
        {
            case GroundType.Type.Grass:
                audioManager.PlaySEOneShot(footAudio, AUDIO.SE_FOOTSTEP_GRASS1, grass_Volume);
                break;
            case GroundType.Type.Rock:
                audioManager.PlaySEOneShot(footAudio, AUDIO.SE_FOOTSTEP_ROCK1, rock_Volume);
                break;
            case GroundType.Type.Snow:
                audioManager.PlaySEOneShot(footAudio, AUDIO.SE_FOOTSTEP_SNOW1, snow1_Volume);
                break;
            case GroundType.Type.Sand:
                audioManager.PlaySEOneShot(footAudio, AUDIO.SE_FOOTSTEP_SAND, sand_Volume);
                break;
        }
    }

    void FootStep2()
    {
        if (!stateManager.onGround) return;
        if (planetWalker.moveAmount < 0.1f) return;
        AudioManager audioManager = AudioManager.Instance;

        switch (stateManager.groundType)
        {
            case GroundType.Type.Grass:
                audioManager.PlaySEOneShot(footAudio, AUDIO.SE_FOOTSTEP_GRASS1, grass_Volume);
                break;
            case GroundType.Type.Rock:
                audioManager.PlaySEOneShot(footAudio, AUDIO.SE_FOOTSTEP_ROCK1, rock_Volume);
                break;
            case GroundType.Type.Snow:
                audioManager.PlaySEOneShot(footAudio, AUDIO.SE_FOOTSTEP_SNOW2, snow2_Volume);
                break;
            case GroundType.Type.Sand:
                audioManager.PlaySEOneShot(footAudio, AUDIO.SE_FOOTSTEP_SAND, sand_Volume);
                break;
        }
    }
}
