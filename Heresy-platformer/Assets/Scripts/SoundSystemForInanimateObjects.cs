using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystemForInanimateObjects : MonoBehaviour
{
    AudioSource myAudioSource;

    [SerializeField]
    private AudioClip[] hitSounds;
    [SerializeField]
    private AudioClip[] destroySounds;

    private void Awake()
    {
        gameObject.AddComponent<AudioSource>();
    }
    private void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    public void PlayOnHitSounds()
    {
        foreach(AudioClip clip in hitSounds)
        {
            myAudioSource.PlayOneShot(hitSounds[RandomSoundFromArray(hitSounds)], 0.3f);
        }
    }
    public void PlayOnDestroySounds()
    {
        foreach(AudioClip clip in destroySounds)
        {
            myAudioSource.PlayOneShot(destroySounds[RandomSoundFromArray(destroySounds)], 0.5f);
        }
    }

    private int RandomSoundFromArray(Array array)
    {
        int randomNumber = UnityEngine.Random.Range(0, array.Length-1);
        return randomNumber;
    }


}
