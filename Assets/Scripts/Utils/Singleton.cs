using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<T>();
            if (instance == null)
                Debug.LogError("Singleton<" + typeof(T) + "> instance has been not found.");
            return instance;
        }
    }

    protected void Awake()
    {
        if (this.GetType() != typeof(T))
            DestroySelf();

        if (instance == null)
            instance = this as T;
        else if (instance != this)
            DestroySelf();
    }

    private void DestroySelf()
    {
        if (Application.isPlaying)
            Destroy(this);
        else
            DestroyImmediate(this);
    }
}