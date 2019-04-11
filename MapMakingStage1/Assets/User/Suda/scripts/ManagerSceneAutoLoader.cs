using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerSceneAutoLoader : MonoBehaviour {

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void LoadSceneManager()
    {
        string SceneManagerName = "SceneManager";

        //SceneManagerが有効でないときに追加ロード
        if (!SceneManager.GetSceneByName(SceneManagerName).IsValid())
        {
            SceneManager.LoadScene(SceneManagerName, LoadSceneMode.Additive);
        }
    }
}
