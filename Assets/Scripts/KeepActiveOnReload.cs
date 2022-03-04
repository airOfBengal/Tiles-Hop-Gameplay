using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepActiveOnReload : MonoBehaviour
{
    public static KeepActiveOnReload instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
    }
}
