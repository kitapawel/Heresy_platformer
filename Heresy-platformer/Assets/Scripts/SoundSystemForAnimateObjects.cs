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
    [SerializeField]
    private AudioClip[] effortSounds;
    [SerializeField]
    private AudioClip[] receiveDamageSounds;
    [SerializeField]
    private AudioClip[] parrySounds;

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
        myAudioSource.PlayOneShot(receiveDamageSounds[RandomSoundFromArray(receiveDamageSounds)], 0.5f);
    }
    public void PlayEffortSounds()
    {
        myAudioSource.PlayOneShot(effortSounds[RandomSoundFromArray(effortSounds)], 0.5f);
    }
    public void PlayParrySounds()
    {
        myAudioSource.PlayOneShot(parrySounds[RandomSoundFromArray(parrySounds)], 0.5f);
    }

    private int RandomSoundFromArray(Array array)
    {
        int randomNumber = UnityEngine.Random.Range(0, array.Length-1);
        return randomNumber;
    }


}
