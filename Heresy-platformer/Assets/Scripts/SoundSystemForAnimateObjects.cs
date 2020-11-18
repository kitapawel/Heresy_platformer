using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystemForAnimateObjects : MonoBehaviour
{
    AudioSource myAudioSource;

    [SerializeField]
    private AudioClip[] footSteps;
    [SerializeField]
    private AudioClip[] weaponSwingSounds;
    [SerializeField]
    private AudioClip[] effortSounds;
    [SerializeField]
    private AudioClip[] painSounds;
    [SerializeField]
    private AudioClip[] parrySounds;
    [SerializeField]
    private AudioClip[] getHitSounds;

    private void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    public void PlayFootsteps()
    {
        myAudioSource.PlayOneShot(footSteps[RandomSoundFromArray(footSteps)], 0.3f);
    }
    public void PlayWeaponSwingSounds()
    {
        myAudioSource.PlayOneShot(weaponSwingSounds[RandomSoundFromArray(weaponSwingSounds)], 0.5f);
    }
    public void PlayPainSounds()
    {
        myAudioSource.PlayOneShot(painSounds[RandomSoundFromArray(painSounds)], 0.5f);
    }
    public void PlayEffortSounds()
    {
        myAudioSource.PlayOneShot(effortSounds[RandomSoundFromArray(effortSounds)], 0.5f);
    }
    public void PlayParrySounds()
    {
        myAudioSource.PlayOneShot(parrySounds[RandomSoundFromArray(parrySounds)], 0.5f);
    }
    public void PlayGetHitSounds()
    {
        myAudioSource.PlayOneShot(getHitSounds[RandomSoundFromArray(getHitSounds)], 0.5f);
    }

    private int RandomSoundFromArray(Array array)
    {
        int randomNumber = UnityEngine.Random.Range(0, array.Length-1);
        return randomNumber;
    }


}
