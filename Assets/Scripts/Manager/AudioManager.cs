using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource music;

    void Awake()
    {
        music = GetComponent<AudioSource>();
        DontDestroyOnLoad(music);
    }
}
