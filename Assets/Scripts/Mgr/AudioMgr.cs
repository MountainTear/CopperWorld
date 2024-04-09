using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMgr : Singleton<AudioMgr>
{
    private AudioSource bgSource;   //±≥æ∞“Ù‘¥
    private AudioSource effectSource;   //“Ù–ß“Ù‘¥
    private AudioClip bgGameStart, bgGameIn, run;


    public AudioMgr()
    {
        InitSourceAndClip();
    }
    
    private void InitSourceAndClip()
    {
        bgSource = GameObject.Find("BgAudio").GetComponent<AudioSource>();
        effectSource = GameObject.Find("EffectAudio").GetComponent<AudioSource>();

        bgGameStart = Resources.Load<AudioClip>("Audios/gameStart");
        bgGameIn = Resources.Load<AudioClip>("Audios/gameIn");
        run = Resources.Load<AudioClip>("Audios/run");
    }

    public void BgGameStartAudio()
    {
        bgSource.clip = bgGameStart;
        bgSource.Play();
    }

    public void BgGameInAudio()
    {
        bgSource.clip = bgGameIn;
        bgSource.Play();
    }

    public void RunAudio()
    {
        effectSource.clip = run;
        effectSource.Play();
    }

}
