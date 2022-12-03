using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private GameController gManager;

    public Sound[] sounds;

    public static AudioManager instance;

    private bool audioEnabled;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.loop = sound.loop;
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;

            sound.source.mute = false;
        }
    }

    private void Start()
    {
        audioEnabled = false;
        ToggleAudio();

        Play("Music");
    }

    private void Update()
    {
        if (!gManager)
        {
            if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>())
            {
                gManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            }
        }
        else
        {
            if (gManager.audioButton != null)
            {
                Image icon = gManager.audioButton.GetComponent<Image>();

                if (audioEnabled)
                {
                    gManager.audioButton.color = new Color(1, 0.2530334f, 0, 1); // normal color

                    icon.color = new Color(1, 1, 1, 1);
                }
                else // if the sound is muted
                {
                    gManager.audioButton.color = new Color(0.5849056f, 0.1490936f, 0, 1); // shaded color

                    icon.color = new Color(0.5f, 0.5f, 0.5f, 1);
                }

            }

            if (Input.GetKeyDown(KeyCode.M)) ToggleAudio();
        }
    }

    public void Play(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);

        //if (gManager) if (gManager.gameOver && !s.playIfGameOver) return;

        if (s != null && audioEnabled)
        {
            if (s.randomPitch) s.source.pitch = UnityEngine.Random.Range(0.6f, 1.4f);

            s.source.Play();
        }
    }

    public void Stop(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);

        if (s != null && audioEnabled)
        {
            s.source.Stop();
        }
    }

    public void ToggleAudio()
    {
        audioEnabled = !audioEnabled;

        foreach (Sound sound in sounds)
        {
            sound.source.mute = !audioEnabled;
        }
    }
}