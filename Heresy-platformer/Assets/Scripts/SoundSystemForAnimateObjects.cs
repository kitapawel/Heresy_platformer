using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class SoundSystemForAnimateObjects : MonoBehaviour
{
    AudioSource myAudioSource;

    [SerializeField]
    private AudioClip[] footSteps;
    [SerializeField]
    private AudioClip[] weaponSwingSounds;

    private void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    public void PlayFootsteps()
    {
        foreach(AudioClip clip in footSteps)
        {
            myAudioSource.PlayOneShot(footSteps[RandomSoundFromArray(footSteps)], 0.3f);
        }
    }
    public void PlayWeaponSwingSounds()
    {
        foreach(AudioClip clip in weaponSwingSounds)
        {
            myAudioSource.PlayOneShot(weaponSwingSounds[RandomSoundFromArray(weaponSwingSounds)], 0.5f);
        }
    }

    private int RandomSoundFromArray(Array array)
    {
        int randomNumber = UnityEngine.Random.Range(0, array.Length-1);
        return randomNumber;
    }


}
