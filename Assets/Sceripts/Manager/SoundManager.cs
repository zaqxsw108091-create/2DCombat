using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingleTon<SoundManager>
{
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioSource bgmAudioSource;
    public void PlaySFXClip(AudioClip clip, float volume)
    {
        sfxAudioSource.clip = clip;
        sfxAudioSource.PlayOneShot(clip);
        sfxAudioSource.volume = volume;
    }
    
    public void PlaySFXFromString(string soundClipName, float volume)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Sound/{soundClipName}");
        sfxAudioSource.clip = clip;
        sfxAudioSource.PlayOneShot(clip);
        sfxAudioSource.volume = volume;
    }


}
