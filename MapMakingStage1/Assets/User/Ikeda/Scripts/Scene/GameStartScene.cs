using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartScene : SceneBase {

    [SerializeField] private Animator animator;

    public override void Start()
    {
        base.Start();
        DataManager.Instance.ReSet();
    }

    public override void Update()
    {
        base.Update();
    }

    public void Next()
    {
        MySceneManager.FadeInLoad(MySceneManager.Instance.Path_GalaxySelect,true);
    }
}
