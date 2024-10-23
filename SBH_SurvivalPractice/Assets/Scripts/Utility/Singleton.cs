using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instacne 
    {
        get 
        {
            if(instance == null)
            {
                instance = new GameObject("Manager").AddComponent<T>();
            }

            return instance;
        } 
    }
    public static bool InstanceExit {  get { return instance != null; } }

    protected virtual void Awake()
    {
        if(InstanceExit)
        {
            Destroy(gameObject);
            return;
        }

        instance = (T)this;
    }

    protected virtual void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;    
        }
    }
}
