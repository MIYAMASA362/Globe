using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryEvent : MonoBehaviour
{
    [SerializeField] private GameStartScene startScene;

    public void Next()
    {
        startScene.Next();
    }

}
