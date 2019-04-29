using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartScene : SceneBase {

    public override void Start()
    {
        base.Start();
        Invoke("Next",2f);
    }

    public override void Update()
    {
        base.Update();
    }

    private void Next()
    {
        MySceneManager.FadeInLoad(MySceneManager.Instance.Path_GalaxySelect);
    }
}
