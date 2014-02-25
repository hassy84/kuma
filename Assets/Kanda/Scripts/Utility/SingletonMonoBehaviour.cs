using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {
    private static T instance;
    public static T Instance {
        get {
            if (instance == null) {
                instance = (T)FindObjectOfType (typeof(T));

                if (instance == null) {
                    Debug.LogError (typeof(T) + "is nothing");
                }
            }

            return instance;
        }
    }

    public static bool hasInstance {
        get {return (instance != null);}
    }

    protected virtual void Awake() {
        CheckInstance();
        DontDestroyOnLoad(transform.root.gameObject);
    }

    protected bool CheckInstance() {
        if ( this == Instance) {
            return true;
        }
        Destroy(this);
        return false;
    }
}
