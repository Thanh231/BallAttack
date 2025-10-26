using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioClip playSound;
    public AudioClip loseSound;
    public AudioClip moveSound;
    public AudioClip updateSound;
    public AudioClip bgSound1;

    public AudioSource source1;
    public AudioSource source2;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            PlayeBGSound();
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    private void PlayeBGSound()
    {
        source1.clip = bgSound1;
        source1.loop = true;
        source1.Play();
    }
    public void Play(AudioClip sound)
    {
        source2.PlayOneShot(sound);
    }

}
