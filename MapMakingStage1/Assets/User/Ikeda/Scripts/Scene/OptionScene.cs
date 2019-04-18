using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionScene : SceneBase {

    public override void Start()
    {
        base.Start();
        

    }

    public override void Update()
    {
        base.Update();

        
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(MySceneManager.TitleScene);
        }
    }
}
