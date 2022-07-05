using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    static DontDestroyOnLoad DontDestroyOnLoadInstance;
    private void Awake()
    {
        if (DontDestroyOnLoadInstance != null) Destroy(gameObject);
        else
        {
            DontDestroyOnLoadInstance = this;
            DontDestroyOnLoad(gameObject);
        }

    }
}
