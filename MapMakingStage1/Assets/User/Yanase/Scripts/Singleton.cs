using UnityEngine;
using System.Collections;


public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("create SingletonBehaviour<" + typeof(T) + ">");
                instance = (T)FindObjectOfType(typeof(T));

                // Hierarchyに存在しなかった場合
                if (instance == null)
                {
                    Debug.Log(typeof(T) + " is nothing");

                    // ゲームオブジェクトを作成しコンポーネントを追加する
                    GameObject obj = new GameObject(typeof(T).Name);
                    instance = (T)obj.AddComponent<T>();
                }
            }

            
            return instance;
        }
    }
}
