using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Glitch
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T s_Instance = null;
        public static T Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindObjectOfType<T>();
                    if (s_Instance == null)
                    {
                        GameObject singleton = new GameObject(typeof(T).Name);
                        //Debug.Log("Create Instance");
                        s_Instance = singleton.AddComponent<T>();
                        //Debug.Log("AddComponet Instance");
                        DontDestroyOnLoad(singleton);
                    }
                }
                return s_Instance;
            }
        }

        protected void awake(T singleObject)
        {
            if (s_Instance == null)
            {
                return;
            }
            if(s_Instance != singleObject)
            {
                Destroy(gameObject);
                Debug.Log("Another Singleton Destory : " + singleObject.name);
            }
        }
    }
}

