using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerScript : MonoBehaviour
{
    private AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();    
    }

    public void PlayAudio(AudioClip audioClip)
    {
        m_audioSource.clip = audioClip;
        m_audioSource.Play();
    }
}
