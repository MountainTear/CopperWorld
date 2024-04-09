using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMgr : Singleton<AudioMgr>
{
    private AudioSource audioSource;
    private AudioClip jump, run, shoot,change;


    public AudioMgr()
    {
        InitSourceAndClip();
    }
    
    private void InitSourceAndClip()
    {
        audioSource = GameObject.Find("AudioParent").GetComponent<AudioSource>();
    }

    public void JumpAudio()
    {
        audioSource.clip = jump;
        audioSource.Play();
    }

    public void RunAudio()
    {
        audioSource.clip = run;
        audioSource.Play();
    }

    public void ShootAudio()
    {
        audioSource.clip = shoot;
        audioSource.Play();
    }

    public void ChangeAudio()
    {
        audioSource.clip = change;
        audioSource.Play();
    }

}
