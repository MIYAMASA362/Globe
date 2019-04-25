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

                // Hierarchy�ɑ��݂��Ȃ������ꍇ
                if (instance == null)
                {
                    Debug.Log(typeof(T) + " is nothing");

                    // �Q�[���I�u�W�F�N�g���쐬���R���|�[�l���g��ǉ�����
                    GameObject obj = new GameObject(typeof(T).Name);
                    instance = (T)obj.AddComponent<T>();
                }
            }

            
            return instance;
        }
    }
}
