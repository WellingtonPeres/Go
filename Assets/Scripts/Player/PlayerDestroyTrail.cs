using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDestroyTrail : MonoBehaviour
{
    public float timeForDestroy;

    void Start()
    {
        Destroy(gameObject, timeForDestroy);
    }
}
