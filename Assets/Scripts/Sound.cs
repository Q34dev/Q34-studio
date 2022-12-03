﻿using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    
    public AudioClip clip;

    public bool loop;

    [Range(0, 1)]
    public float volume;

    [Range(0.1f, 3f)]
    public float pitch;

    public bool randomPitch;

    //public bool playIfGameOver;

    [HideInInspector]
    public AudioSource source;
}