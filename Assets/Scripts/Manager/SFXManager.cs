using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    [Header("Array GameObjects SFX")]
    public GameObject[] arraySFX;

    private void Start()
    {
        instance = this;
    }

    public void PlaySFX()
    {
        int randSFX = Random.Range(0, arraySFX.Length);
        Instantiate(arraySFX[randSFX], transform.position, Quaternion.identity);
    }
}
