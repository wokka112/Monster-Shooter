using UnityEngine;
using System;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
            return;
        }

        // Set the sound attributes for each sound in the manager
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup = s.group;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        // If sound does not exist
        if (s == null)
        {
            // Log a warning
            Debug.LogWarning("Sound: " + name + " was not found!");
            return;
        }

        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        // If sound does not exist
        if (s == null)
        {
            // Log a warning
            Debug.LogWarning("Sound: " + name + " was not found!");
            return;
        }

        s.source.Stop();
    }
}
